import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import Swal from 'sweetalert2'
import type { CreateCustomerDTO, CustomerInterest } from '../../types'
import { customerService } from '../../services/customerService'

interface FormState {
  name: string
  email: string
  phone: string
  mainInterest: string
}

const EMPTY_FORM: FormState = {
  name: '',
  email: '',
  phone: '',
  mainInterest: '',
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
  if (digits.length <= 7) return `(${digits.slice(0, 2)}) ${digits.slice(2)}`
  return `(${digits.slice(0, 2)}) ${digits.slice(2, 7)}-${digits.slice(7)}`
}

export default function NewCustomer() {
  const navigate = useNavigate()
  const [form, setForm] = useState<FormState>(EMPTY_FORM)
  const [loading, setLoading] = useState(false)

  function handleChange(e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) {
    const { name, value } = e.target
    setForm(prev => ({ ...prev, [name]: value }))
  }

  function handlePhoneChange(e: React.ChangeEvent<HTMLInputElement>) {
    setForm(prev => ({ ...prev, phone: formatPhone(e.target.value) }))
  }

  function handleClear() {
    setForm(EMPTY_FORM)
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault()

    const { name, email, phone, mainInterest } = form

    if (!name || !email || !phone || !mainInterest) {
      Swal.fire({
        icon: 'warning',
        title: 'Atenção',
        text: 'Preencha todos os campos obrigatórios.',
        confirmButtonText: 'OK',
      })
      return
    }

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
    if (!emailRegex.test(email)) {
      Swal.fire({
        icon: 'warning',
        title: 'Atenção',
        text: 'Informe um e-mail válido.',
        confirmButtonText: 'OK',
      })
      return
    }

    const payload: CreateCustomerDTO = {
      name,
      email,
      phone,
      mainInterest: parseInt(mainInterest, 10) as CustomerInterest,
    }

    try {
      setLoading(true)
      await customerService.create(payload)
      await Swal.fire({
        icon: 'success',
        title: 'Sucesso!',
        text: 'Cliente cadastrado com sucesso!',
        confirmButtonText: 'OK',
      })
      navigate('/customers')
    } catch (error: unknown) {
      const detail =
        (error as { response?: { data?: { detail?: string } } })?.response?.data?.detail
      Swal.fire({
        icon: 'error',
        title: 'Erro',
        text: detail ?? 'Erro ao cadastrar cliente. Tente novamente.',
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
          onClick={() => navigate('/customers')}
          type="button"
        >
          <i className="bi bi-arrow-left me-1" />
          Voltar
        </button>
        <h1 className="h3 mb-0">Novo Cliente</h1>
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
                  E-mail <span className="text-danger">*</span>
                </label>
                <input
                  id="email"
                  name="email"
                  type="email"
                  className="form-control"
                  placeholder="Ex: joao@email.com"
                  value={form.email}
                  onChange={handleChange}
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
                disabled={loading}
              >
                {loading ? (
                  <>
                    <span className="spinner-border spinner-border-sm me-2" role="status" />
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
                onClick={handleClear}
                disabled={loading}
              >
                <i className="bi bi-x-lg me-1" />
                Limpar
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  )
}
