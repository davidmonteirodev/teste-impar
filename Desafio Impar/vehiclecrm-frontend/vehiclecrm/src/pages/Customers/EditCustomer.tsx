import { useState, useEffect } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import Swal from 'sweetalert2'
import type { UpdateCustomerDTO, CustomerInterest } from '../../types'
import { customerService } from '../../services/customerService'

interface FormState {
  name: string
  email: string
  phone: string
  mainInterest: string
}

const INTEREST_OPTIONS: { value: CustomerInterest; label: string }[] = [
  { value: 1, label: 'SUV' },
  { value: 2, label: 'Hatch' },
  { value: 3, label: 'Sedan' },
  { value: 4, label: 'Utilitário' },
  { value: 5, label: 'Carro Usado' },
  { value: 6, label: 'Carro Novo' },
]

function formatPhone(value: string): string {
  const digits = value.replace(/\D/g, '').slice(0, 11)
  if (digits.length <= 2) return digits.length ? `(${digits}` : ''
  if (digits.length <= 6) return `(${digits.slice(0, 2)}) ${digits.slice(2)}`
  if (digits.length <= 10) return `(${digits.slice(0, 2)}) ${digits.slice(2, 6)}-${digits.slice(6)}`
  return `(${digits.slice(0, 2)}) ${digits.slice(2, 7)}-${digits.slice(7)}`
}

export default function EditCustomer() {
  const { id } = useParams<{ id: string }>()
  const navigate = useNavigate()
  const [form, setForm] = useState<FormState | null>(null)
  const [loadingData, setLoadingData] = useState(true)
  const [saving, setSaving] = useState(false)

  useEffect(() => {
    if (!id) return
    customerService
      .getById(Number(id))
      .then(res => {
        const c = res.data as any
        setForm({
          name: c.name ?? '',
          email: c.email ?? '',
          phone: formatPhone(c.phone ?? ''),
          mainInterest: String(c.mainInterest ?? ''),
        })
      })
      .catch(() => {
        Swal.fire({
          icon: 'error',
          title: 'Erro',
          text: 'Não foi possível carregar os dados do cliente.',
          confirmButtonText: 'OK',
        }).then(() => navigate('/customers'))
      })
      .finally(() => setLoadingData(false))
  }, [id, navigate])

  function handleChange(e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) {
    const { name, value } = e.target
    setForm(prev => prev ? { ...prev, [name]: value } : prev)
  }

  function handlePhoneChange(e: React.ChangeEvent<HTMLInputElement>) {
    setForm(prev => prev ? { ...prev, phone: formatPhone(e.target.value) } : prev)
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault()
    if (!form) return

    const { name, email, phone, mainInterest } = form

    if (!name || !phone || !mainInterest) {
      Swal.fire({
        icon: 'warning',
        title: 'Atenção',
        text: 'Preencha todos os campos obrigatórios.',
        confirmButtonText: 'OK',
      })
      return
    }

    const payload: UpdateCustomerDTO = {
      name,
      phone: phone.replace(/\D/g, ''),
      mainInterest: parseInt(mainInterest, 10) as CustomerInterest,
    }

    try {
      setSaving(true)
      await customerService.update(Number(id), payload)
      await Swal.fire({
        icon: 'success',
        title: 'Sucesso!',
        text: 'Cliente atualizado com sucesso!',
        confirmButtonText: 'OK',
      })
      navigate('/customers')
    } catch (err: any) {
      const message =
        err?.response?.data?.detail ??
        err?.response?.data?.message ??
        err?.response?.data ??
        'Erro ao salvar o cliente. Tente novamente.'
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
        <span className="ms-3 text-muted">Carregando dados do cliente...</span>
      </div>
    )
  }

  if (!form) return null

  return (
    <div>
      <div className="d-flex align-items-center gap-2 mb-4">
        <button
          className="btn btn-outline-secondary btn-sm"
          onClick={() => navigate('/customers')}
          type="button"
        >
          <i className="bi bi-arrow-left me-1" />
          Voltar
        </button>
        <h1 className="h3 mb-0">Editar Cliente</h1>
      </div>

      <div className="card shadow-sm">
        <div className="card-body">
          <form onSubmit={handleSubmit} noValidate>
            <div className="row g-3">
              <div className="col-md-6">
                <label htmlFor="name" className="form-label fw-semibold">
                  Nome <span className="text-danger">*</span>
                </label>
                <input
                  id="name"
                  name="name"
                  type="text"
                  className="form-control"
                  placeholder="Ex: João Silva"
                  value={form.name}
                  onChange={handleChange}
                />
              </div>

              <div className="col-md-6">
                <label htmlFor="email" className="form-label fw-semibold">
                  E-mail
                </label>
                <input
                  id="email"
                  name="email"
                  type="email"
                  className="form-control"
                  value={form.email}
                  disabled
                />
              </div>

              <div className="col-md-6">
                <label htmlFor="phone" className="form-label fw-semibold">
                  Telefone <span className="text-danger">*</span>
                </label>
                <input
                  id="phone"
                  name="phone"
                  type="text"
                  className="form-control"
                  placeholder="Ex: (11) 91234-5678"
                  value={form.phone}
                  onChange={handlePhoneChange}
                  maxLength={15}
                />
              </div>

              <div className="col-md-6">
                <label htmlFor="mainInterest" className="form-label fw-semibold">
                  Interesse Principal <span className="text-danger">*</span>
                </label>
                <select
                  id="mainInterest"
                  name="mainInterest"
                  className="form-select"
                  value={form.mainInterest}
                  onChange={handleChange}
                >
                  <option value="">Selecione...</option>
                  {INTEREST_OPTIONS.map(opt => (
                    <option key={opt.value} value={opt.value}>
                      {opt.label}
                    </option>
                  ))}
                </select>
              </div>
            </div>

            <div className="d-flex gap-2 mt-4">
              <button
                type="submit"
                className="btn btn-primary"
                disabled={saving}
              >
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
                onClick={() => navigate('/customers')}
                disabled={saving}
              >
                <i className="bi bi-x-lg me-1" />
                Cancelar
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  )
}
