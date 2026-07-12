import { useState, useEffect } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import Swal from 'sweetalert2'
import type { UpdateVehicleDTO, VehicleStatus } from '../../types'
import { vehicleService } from '../../services/vehicleService'

interface FormState {
  brand: string
  model: string
  year: string
  priceDigits: string
  color: string
  mileage: string
  status: string
}

function formatBRL(digits: string): string {
  if (!digits) return ''
  const value = parseInt(digits.padStart(3, '0'), 10)
  return (value / 100).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })
}

export default function EditVehicle() {
  const { id } = useParams<{ id: string }>()
  const navigate = useNavigate()
  const [form, setForm] = useState<FormState | null>(null)
  const [loadingData, setLoadingData] = useState(true)
  const [saving, setSaving] = useState(false)

  useEffect(() => {
    if (!id) return
    vehicleService
      .getById(Number(id))
      .then(res => {
        const v = res.data as any
        setForm({
          brand: v.brand ?? '',
          model: v.model ?? '',
          year: String(v.year ?? ''),
          priceDigits: String(Math.round((v.price ?? 0) * 100)),
          color: v.color ?? '',
          mileage: String(v.mileage ?? ''),
          status: String(v.status ?? ''),
        })
      })
      .catch(() => {
        Swal.fire({
          icon: 'error',
          title: 'Erro',
          text: 'Não foi possível carregar os dados do veículo.',
          confirmButtonText: 'OK',
        }).then(() => navigate('/vehicles'))
      })
      .finally(() => setLoadingData(false))
  }, [id, navigate])

  function handleChange(e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) {
    const { name, value } = e.target
    setForm(prev => prev ? { ...prev, [name]: value } : prev)
  }

  function handlePriceChange(e: React.ChangeEvent<HTMLInputElement>) {
    const digits = e.target.value.replace(/\D/g, '')
    setForm(prev => prev ? { ...prev, priceDigits: digits } : prev)
  }

  function handleYearChange(e: React.ChangeEvent<HTMLInputElement>) {
    const digits = e.target.value.replace(/\D/g, '').slice(0, 4)
    setForm(prev => prev ? { ...prev, year: digits } : prev)
  }

  function handleMileageChange(e: React.ChangeEvent<HTMLInputElement>) {
    const digits = e.target.value.replace(/\D/g, '')
    setForm(prev => prev ? { ...prev, mileage: digits } : prev)
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault()
    if (!form) return

    const { brand, model, year, priceDigits, color, mileage, status } = form

    if (!brand || !model || !year || !priceDigits || !color || !mileage || !status) {
      Swal.fire({
        icon: 'warning',
        title: 'Atenção',
        text: 'Preencha todos os campos obrigatórios.',
        confirmButtonText: 'OK',
      })
      return
    }

    if (year.length !== 4) {
      Swal.fire({
        icon: 'warning',
        title: 'Atenção',
        text: 'O ano deve conter exatamente 4 dígitos.',
        confirmButtonText: 'OK',
      })
      return
    }

    const payload: UpdateVehicleDTO = {
      brand,
      model,
      year: parseInt(year, 10),
      price: parseInt(priceDigits, 10) / 100,
      color,
      mileage: parseInt(mileage, 10),
      status: parseInt(status, 10) as VehicleStatus,
    }

    try {
      setSaving(true)
      await vehicleService.update(Number(id), payload)
      await Swal.fire({
        icon: 'success',
        title: 'Sucesso!',
        text: 'Veículo atualizado com sucesso!',
        confirmButtonText: 'OK',
      })
      navigate('/vehicles')
    } catch (err: any) {
      const message =
        err?.response?.data?.message ??
        err?.response?.data ??
        'Erro ao salvar o veículo. Tente novamente.'
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
        <span className="ms-3 text-muted">Carregando dados do veículo...</span>
      </div>
    )
  }

  if (!form) return null

  return (
    <div>
      <div className="d-flex align-items-center gap-2 mb-4">
        <button
          className="btn btn-outline-secondary btn-sm"
          onClick={() => navigate('/vehicles')}
          type="button"
        >
          <i className="bi bi-arrow-left me-1" />
          Voltar
        </button>
        <h1 className="h3 mb-0">Editar Veículo</h1>
      </div>

      <div className="card shadow-sm">
        <div className="card-body">
          <form onSubmit={handleSubmit} noValidate>
            <div className="row g-3">
              <div className="col-md-6">
                <label htmlFor="brand" className="form-label fw-semibold">
                  Marca <span className="text-danger">*</span>
                </label>
                <input
                  id="brand"
                  name="brand"
                  type="text"
                  className="form-control"
                  placeholder="Ex: Toyota"
                  value={form.brand}
                  onChange={handleChange}
                />
              </div>

              <div className="col-md-6">
                <label htmlFor="model" className="form-label fw-semibold">
                  Modelo <span className="text-danger">*</span>
                </label>
                <input
                  id="model"
                  name="model"
                  type="text"
                  className="form-control"
                  placeholder="Ex: Corolla"
                  value={form.model}
                  onChange={handleChange}
                />
              </div>

              <div className="col-md-4">
                <label htmlFor="year" className="form-label fw-semibold">
                  Ano <span className="text-danger">*</span>
                </label>
                <input
                  id="year"
                  name="year"
                  type="text"
                  inputMode="numeric"
                  className="form-control"
                  placeholder="Ex: 2024"
                  maxLength={4}
                  value={form.year}
                  onChange={handleYearChange}
                />
              </div>

              <div className="col-md-4">
                <label htmlFor="price" className="form-label fw-semibold">
                  Preço <span className="text-danger">*</span>
                </label>
                <input
                  id="price"
                  name="price"
                  type="text"
                  inputMode="numeric"
                  className="form-control"
                  placeholder="R$ 0,00"
                  value={formatBRL(form.priceDigits)}
                  onChange={handlePriceChange}
                />
              </div>

              <div className="col-md-4">
                <label htmlFor="color" className="form-label fw-semibold">
                  Cor <span className="text-danger">*</span>
                </label>
                <input
                  id="color"
                  name="color"
                  type="text"
                  className="form-control"
                  placeholder="Ex: Prata"
                  value={form.color}
                  onChange={handleChange}
                />
              </div>

              <div className="col-md-6">
                <label htmlFor="mileage" className="form-label fw-semibold">
                  Quilometragem <span className="text-danger">*</span>
                </label>
                <div className="input-group">
                  <input
                    id="mileage"
                    name="mileage"
                    type="text"
                    inputMode="numeric"
                    className="form-control"
                    placeholder="Ex: 50000"
                    value={form.mileage}
                    onChange={handleMileageChange}
                  />
                  <span className="input-group-text">km</span>
                </div>
              </div>

              <div className="col-md-6">
                <label htmlFor="status" className="form-label fw-semibold">
                  Status <span className="text-danger">*</span>
                </label>
                <select
                  id="status"
                  name="status"
                  className="form-select"
                  value={form.status}
                  onChange={handleChange}
                >
                  <option value="">Selecione o status</option>
                  <option value="1">Disponível</option>
                  <option value="2">Reservado</option>
                  <option value="3">Vendido</option>
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
                onClick={() => navigate('/vehicles')}
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
