import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import Swal from 'sweetalert2'
import type { CreateVehicleDTO } from '../../types'
import { vehicleService } from '../../services/vehicleService'

interface FormState {
  brand: string
  model: string
  year: string
  priceDigits: string
  color: string
  mileage: string
}

const EMPTY_FORM: FormState = {
  brand: '',
  model: '',
  year: '',
  priceDigits: '',
  color: '',
  mileage: '',
}

function formatBRL(digits: string): string {
  if (!digits) return ''
  const value = parseInt(digits.padStart(3, '0'), 10)
  return (value / 100).toLocaleString('pt-BR', {
    style: 'currency',
    currency: 'BRL',
  })
}

export default function NewVehicle() {
  const navigate = useNavigate()
  const [form, setForm] = useState<FormState>(EMPTY_FORM)
  const [loading, setLoading] = useState(false)

  function handleChange(e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) {
    const { name, value } = e.target
    setForm(prev => ({ ...prev, [name]: value }))
  }

  function handlePriceChange(e: React.ChangeEvent<HTMLInputElement>) {
    const digits = e.target.value.replace(/\D/g, '')
    setForm(prev => ({ ...prev, priceDigits: digits }))
  }

  function handleYearChange(e: React.ChangeEvent<HTMLInputElement>) {
    const digits = e.target.value.replace(/\D/g, '').slice(0, 4)
    setForm(prev => ({ ...prev, year: digits }))
  }

  function handleMileageChange(e: React.ChangeEvent<HTMLInputElement>) {
    const digits = e.target.value.replace(/\D/g, '')
    setForm(prev => ({ ...prev, mileage: digits }))
  }

  function handleClear() {
    setForm(EMPTY_FORM)
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault()

    const { brand, model, year, priceDigits, color, mileage } = form

    if (!brand || !model || !year || !priceDigits || !color || !mileage) {
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

    const payload: CreateVehicleDTO = {
      brand,
      model,
      year: parseInt(year, 10),
      price: parseInt(priceDigits, 10) / 100,
      color,
      mileage: parseInt(mileage, 10),
    }

    try {
      setLoading(true)
      await vehicleService.create(payload)
      await Swal.fire({
        icon: 'success',
        title: 'Sucesso!',
        text: 'Veículo cadastrado com sucesso!',
        confirmButtonText: 'OK',
      })
      navigate('/vehicles')
    } catch (err: any) {
      const message =
        err?.response?.data?.message ??
        err?.response?.data ??
        'Erro ao cadastrar veículo. Tente novamente.'
      Swal.fire({
        icon: 'error',
        title: 'Erro',
        text: typeof message === 'string' ? message : JSON.stringify(message),
        confirmButtonText: 'OK',
      })
    } finally {
      setLoading(false)
    }
  }

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
        <h1 className="h3 mb-0">Novo Veículo</h1>
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

              <div className="col-md-3">
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

              <div className="col-md-3">
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

              <div className="col-md-3">
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

              <div className="col-md-3">
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
            </div>

            <div className="d-flex gap-2 mt-4">
              <button
                type="submit"
                className="btn btn-primary"
                disabled={loading}
              >
                {loading ? (
                  <>
                    <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true" />
                    Cadastrando...
                  </>
                ) : (
                  <>
                    <i className="bi bi-check-lg me-1" />
                    Cadastrar
                  </>
                )}
              </button>
              <button
                type="button"
                className="btn btn-outline-secondary"
                onClick={handleClear}
                disabled={loading}
              >
                <i className="bi bi-eraser me-1" />
                Limpar
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  )
}
