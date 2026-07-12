import { useState, useEffect, useRef } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import Swal from 'sweetalert2'
import type { UpdateSaleOpportunityDTO, SaleOpportunityStatus, Vehicle } from '../../types'
import { saleOpportunityService } from '../../services/saleOpportunityService'
import { customerService } from '../../services/customerService'
import { vehicleService } from '../../services/vehicleService'
import type { Customer } from '../../types'

const STATUS_OPTIONS: { value: SaleOpportunityStatus; label: string }[] = [
  { value: 1, label: 'Novo lead' },
  { value: 2, label: 'Em negociação' },
  { value: 3, label: 'Proposta enviada' },
  { value: 4, label: 'Vendido' },
  { value: 5, label: 'Perdido' },
]

function formatBRL(digits: string): string {
  if (!digits) return ''
  const value = parseInt(digits.padStart(3, '0'), 10)
  return (value / 100).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })
}

interface FormState {
  customerId: number | null
  vehicleId: number | null
  status: string
  proposedValueDigits: string
  notes: string
}

export default function EditSaleOpportunity() {
  const { id } = useParams<{ id: string }>()
  const navigate = useNavigate()

  const [form, setForm] = useState<FormState | null>(null)
  const [loadingData, setLoadingData] = useState(true)
  const [saving, setSaving] = useState(false)

  // Customer autocomplete
  const [customerInput, setCustomerInput] = useState('')
  const [customerSuggestions, setCustomerSuggestions] = useState<Customer[]>([])
  const [showCustomerSuggestions, setShowCustomerSuggestions] = useState(false)
  const [searchingCustomers, setSearchingCustomers] = useState(false)
  const customerDebounceRef = useRef<ReturnType<typeof setTimeout> | null>(null)
  const customerAutocompleteRef = useRef<HTMLDivElement>(null)
  const customerTypingRef = useRef(false)

  // Vehicle autocomplete
  const [vehicleInput, setVehicleInput] = useState('')
  const [vehicleSuggestions, setVehicleSuggestions] = useState<Vehicle[]>([])
  const [showVehicleSuggestions, setShowVehicleSuggestions] = useState(false)
  const [searchingVehicles, setSearchingVehicles] = useState(false)
  const vehicleDebounceRef = useRef<ReturnType<typeof setTimeout> | null>(null)
  const vehicleAutocompleteRef = useRef<HTMLDivElement>(null)
  const vehicleTypingRef = useRef(false)

  // Load existing data
  useEffect(() => {
    if (!id) return
    saleOpportunityService
      .getById(Number(id))
      .then(res => {
        const o = res.data
        setForm({
          customerId: o.customer.customerId,
          vehicleId: o.vehicle.vehicleId,
          status: String(o.status),
          proposedValueDigits: String(Math.round(o.proposedValue * 100)),
          notes: o.notes ?? '',
        })
        setCustomerInput(o.customer.name)
        setVehicleInput(o.vehicle.model)
      })
      .catch(() => {
        Swal.fire({
          icon: 'error',
          title: 'Erro',
          text: 'Não foi possível carregar os dados da oportunidade.',
          confirmButtonText: 'OK',
        }).then(() => navigate('/sale-opportunities'))
      })
      .finally(() => setLoadingData(false))
  }, [id, navigate])

  // Close dropdowns when clicking outside
  useEffect(() => {
    function handleClickOutside(e: MouseEvent) {
      if (customerAutocompleteRef.current && !customerAutocompleteRef.current.contains(e.target as Node)) {
        setShowCustomerSuggestions(false)
      }
      if (vehicleAutocompleteRef.current && !vehicleAutocompleteRef.current.contains(e.target as Node)) {
        setShowVehicleSuggestions(false)
      }
    }
    document.addEventListener('mousedown', handleClickOutside)
    return () => document.removeEventListener('mousedown', handleClickOutside)
  }, [])

  // Debounced customer search
  useEffect(() => {
    if (!customerTypingRef.current) return
    if (customerInput.length < 2) {
      setCustomerSuggestions([])
      setShowCustomerSuggestions(false)
      if (customerDebounceRef.current) clearTimeout(customerDebounceRef.current)
      return
    }
    if (customerDebounceRef.current) clearTimeout(customerDebounceRef.current)
    customerDebounceRef.current = setTimeout(async () => {
      setSearchingCustomers(true)
      try {
        const res = await customerService.getCustomers({ name: customerInput, page: 1, pageSize: 10 })
        setCustomerSuggestions(res.data.items)
        setShowCustomerSuggestions(true)
      } catch {
        setCustomerSuggestions([])
      } finally {
        setSearchingCustomers(false)
      }
    }, 1000)
    return () => { if (customerDebounceRef.current) clearTimeout(customerDebounceRef.current) }
  }, [customerInput])

  // Debounced vehicle search
  useEffect(() => {
    if (!vehicleTypingRef.current) return
    if (vehicleInput.length < 2) {
      setVehicleSuggestions([])
      setShowVehicleSuggestions(false)
      if (vehicleDebounceRef.current) clearTimeout(vehicleDebounceRef.current)
      return
    }
    if (vehicleDebounceRef.current) clearTimeout(vehicleDebounceRef.current)
    vehicleDebounceRef.current = setTimeout(async () => {
      setSearchingVehicles(true)
      try {
        const res = await vehicleService.getVehicles({ model: vehicleInput, page: 1, pageSize: 10 })
        setVehicleSuggestions(res.data.items)
        setShowVehicleSuggestions(true)
      } catch {
        setVehicleSuggestions([])
      } finally {
        setSearchingVehicles(false)
      }
    }, 1000)
    return () => { if (vehicleDebounceRef.current) clearTimeout(vehicleDebounceRef.current) }
  }, [vehicleInput])

  function handleCustomerInputChange(e: React.ChangeEvent<HTMLInputElement>) {
    customerTypingRef.current = true
    setCustomerInput(e.target.value)
    setForm(prev => prev ? { ...prev, customerId: null } : prev)
  }

  function handleSelectCustomer(customer: Customer) {
    customerTypingRef.current = false
    setCustomerInput(customer.name)
    setForm(prev => prev ? { ...prev, customerId: customer.id } : prev)
    setShowCustomerSuggestions(false)
    setCustomerSuggestions([])
  }

  function handleVehicleInputChange(e: React.ChangeEvent<HTMLInputElement>) {
    vehicleTypingRef.current = true
    setVehicleInput(e.target.value)
    setForm(prev => prev ? { ...prev, vehicleId: null } : prev)
  }

  function handleSelectVehicle(vehicle: Vehicle) {
    vehicleTypingRef.current = false
    setVehicleInput(`${vehicle.model} (${vehicle.year})`)
    setForm(prev => prev ? { ...prev, vehicleId: vehicle.id } : prev)
    setShowVehicleSuggestions(false)
    setVehicleSuggestions([])
  }

  function handleProposedValueChange(e: React.ChangeEvent<HTMLInputElement>) {
    const digits = e.target.value.replace(/\D/g, '')
    setForm(prev => prev ? { ...prev, proposedValueDigits: digits } : prev)
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault()
    if (!form) return

    const { customerId, vehicleId, status, proposedValueDigits } = form

    if (!customerId || !vehicleId || !status || !proposedValueDigits) {
      Swal.fire({ icon: 'warning', title: 'Atenção', text: 'Preencha todos os campos obrigatórios.', confirmButtonText: 'OK' })
      return
    }

    const payload: UpdateSaleOpportunityDTO = {
      id: Number(id),
      customerId,
      vehicleId,
      status: parseInt(status, 10) as SaleOpportunityStatus,
      proposedValue: parseInt(proposedValueDigits, 10) / 100,
      notes: form.notes || undefined,
    }

    try {
      setSaving(true)
      await saleOpportunityService.update(payload)
      await Swal.fire({
        icon: 'success',
        title: 'Sucesso!',
        text: 'Oportunidade de venda atualizada com sucesso!',
        confirmButtonText: 'OK',
      })
      navigate('/sale-opportunities')
    } catch (err: any) {
      const message =
        err?.response?.data?.message ??
        err?.response?.data ??
        'Erro ao salvar a oportunidade. Tente novamente.'
      Swal.fire({
        icon: 'error',
        title: 'Erro',
        text: typeof message === 'string' ? message : JSON.stringify(message),
        confirmButtonText: 'OK',
      })
    } finally {
      setSaving(false)
    }
  }

  if (loadingData) {
    return (
      <div className="d-flex justify-content-center align-items-center py-5">
        <div className="spinner-border text-primary" role="status" />
        <span className="ms-3 text-muted">Carregando dados da oportunidade...</span>
      </div>
    )
  }

  if (!form) return null

  return (
    <div>
      <div className="d-flex align-items-center gap-2 mb-4">
        <button
          className="btn btn-outline-secondary btn-sm"
          onClick={() => navigate('/sale-opportunities')}
          type="button"
        >
          <i className="bi bi-arrow-left me-1" />
          Voltar
        </button>
        <h1 className="h3 mb-0">Editar Oportunidade de Venda</h1>
      </div>

      <div className="card shadow-sm">
        <div className="card-body">
          <form onSubmit={handleSubmit} noValidate>
            <div className="row g-3">

              {/* Customer autocomplete */}
              <div className="col-md-6">
                <label htmlFor="customerName" className="form-label fw-semibold">
                  Nome do cliente <span className="text-danger">*</span>
                </label>
                <div className="position-relative" ref={customerAutocompleteRef}>
                  <input
                    id="customerName"
                    type="text"
                    className="form-control"
                    placeholder="Digite ao menos 2 caracteres..."
                    value={customerInput}
                    onChange={handleCustomerInputChange}
                    autoComplete="off"
                  />
                  {searchingCustomers && (
                    <div className="position-absolute end-0 top-50 translate-middle-y me-2" style={{ pointerEvents: 'none' }}>
                      <span className="spinner-border spinner-border-sm text-secondary" role="status" />
                    </div>
                  )}
                  {showCustomerSuggestions && (
                    <ul className="dropdown-menu show w-100 mt-1" style={{ maxHeight: '220px', overflowY: 'auto' }}>
                      {customerSuggestions.length > 0
                        ? customerSuggestions.map(c => (
                            <li key={c.id}>
                              <button type="button" className="dropdown-item" onMouseDown={() => handleSelectCustomer(c)}>
                                <span className="fw-semibold">{c.name}</span>
                                <small className="text-muted ms-2">{c.email}</small>
                              </button>
                            </li>
                          ))
                        : !searchingCustomers && (
                            <li><span className="dropdown-item text-muted">Nenhum cliente encontrado</span></li>
                          )}
                    </ul>
                  )}
                </div>
              </div>

              {/* Vehicle autocomplete */}
              <div className="col-md-6">
                <label htmlFor="vehicleModel" className="form-label fw-semibold">
                  Modelo do veículo <span className="text-danger">*</span>
                </label>
                <div className="position-relative" ref={vehicleAutocompleteRef}>
                  <input
                    id="vehicleModel"
                    type="text"
                    className="form-control"
                    placeholder="Digite ao menos 2 caracteres..."
                    value={vehicleInput}
                    onChange={handleVehicleInputChange}
                    autoComplete="off"
                  />
                  {searchingVehicles && (
                    <div className="position-absolute end-0 top-50 translate-middle-y me-2" style={{ pointerEvents: 'none' }}>
                      <span className="spinner-border spinner-border-sm text-secondary" role="status" />
                    </div>
                  )}
                  {showVehicleSuggestions && (
                    <ul className="dropdown-menu show w-100 mt-1" style={{ maxHeight: '220px', overflowY: 'auto' }}>
                      {vehicleSuggestions.length > 0
                        ? vehicleSuggestions.map(v => (
                            <li key={v.id}>
                              <button type="button" className="dropdown-item" onMouseDown={() => handleSelectVehicle(v)}>
                                <span className="fw-semibold">{v.model}</span>
                                <small className="text-muted ms-2">{v.brand} · {v.year}</small>
                              </button>
                            </li>
                          ))
                        : !searchingVehicles && (
                            <li><span className="dropdown-item text-muted">Nenhum veículo encontrado</span></li>
                          )}
                    </ul>
                  )}
                </div>
              </div>

              {/* Status */}
              <div className="col-md-6">
                <label htmlFor="status" className="form-label fw-semibold">
                  Status <span className="text-danger">*</span>
                </label>
                <select
                  id="status"
                  className="form-select"
                  value={form.status}
                  onChange={e => setForm(prev => prev ? { ...prev, status: e.target.value } : prev)}
                >
                  <option value="">Selecione o status</option>
                  {STATUS_OPTIONS.map(opt => (
                    <option key={opt.value} value={opt.value}>{opt.label}</option>
                  ))}
                </select>
              </div>

              {/* Proposed value */}
              <div className="col-md-6">
                <label htmlFor="proposedValue" className="form-label fw-semibold">
                  Valor proposto <span className="text-danger">*</span>
                </label>
                <input
                  id="proposedValue"
                  type="text"
                  inputMode="numeric"
                  className="form-control"
                  placeholder="R$ 0,00"
                  value={formatBRL(form.proposedValueDigits)}
                  onChange={handleProposedValueChange}
                />
              </div>

              {/* Notes */}
              <div className="col-12">
                <label htmlFor="notes" className="form-label fw-semibold">
                  Observações
                </label>
                <textarea
                  id="notes"
                  className="form-control"
                  rows={3}
                  placeholder="Observações sobre a oportunidade..."
                  value={form.notes}
                  onChange={e => setForm(prev => prev ? { ...prev, notes: e.target.value } : prev)}
                />
              </div>
            </div>

            <div className="d-flex gap-2 mt-4">
              <button type="submit" className="btn btn-primary" disabled={saving}>
                {saving ? (
                  <>
                    <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true" />
                    Salvando...
                  </>
                ) : (
                  <>
                    <i className="bi bi-check-lg me-1" />
                    Salvar
                  </>
                )}
              </button>
              <button
                type="button"
                className="btn btn-outline-secondary"
                onClick={() => navigate('/sale-opportunities')}
                disabled={saving}
              >
                Cancelar
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  )
}
