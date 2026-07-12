import { useEffect, useState } from 'react'
import { customerService } from '../../services/customerService'
import type { Customer } from '../../types'

interface Props {
  customerId: number | null
  onClose: () => void
}

const INTEREST_LABELS: Record<number, string> = {
  1: 'SUV',
  2: 'Hatch',
  3: 'Sedan',
  4: 'Utilitário',
  5: 'Carro Usado',
  6: 'Carro Novo',
}

function formatPhone(value: string | null | undefined): string {
  if (!value) return '—'
  const digits = value.replace(/\D/g, '')
  if (digits.length === 11) {
    return `(${digits.slice(0, 2)}) ${digits.slice(2, 7)}-${digits.slice(7)}`
  }
  if (digits.length === 10) {
    return `(${digits.slice(0, 2)}) ${digits.slice(2, 6)}-${digits.slice(6)}`
  }
  return value
}

function formatDate(value: string | null | undefined): string {
  if (!value) return '—'
  return new Date(value).toLocaleString('pt-BR')
}

export default function CustomerDetailModal({ customerId, onClose }: Props) {
  const [customer, setCustomer] = useState<Customer | null>(null)
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState(false)

  useEffect(() => {
    if (customerId === null) {
      setCustomer(null)
      return
    }
    let cancelled = false
    setLoading(true)
    setError(false)
    customerService
      .getById(customerId)
      .then(res => {
        if (!cancelled) setCustomer(res.data)
      })
      .catch(() => {
        if (!cancelled) setError(true)
      })
      .finally(() => {
        if (!cancelled) setLoading(false)
      })
    return () => {
      cancelled = true
    }
  }, [customerId])

  if (customerId === null) return null

  return (
    <>
      {/* Backdrop */}
      <div className="modal-backdrop fade show" onClick={onClose} />

      {/* Modal */}
      <div
        className="modal fade show d-block"
        role="dialog"
        aria-modal="true"
        onClick={e => e.target === e.currentTarget && onClose()}
      >
        <div className="modal-dialog modal-lg modal-dialog-centered">
          <div className="modal-content">
            <div className="modal-header">
              <h5 className="modal-title">
                <i className="bi bi-person me-2" />
                Detalhes do Cliente
              </h5>
              <button
                type="button"
                className="btn-close"
                aria-label="Fechar"
                onClick={onClose}
              />
            </div>

            <div className="modal-body">
              {loading && (
                <div className="text-center py-4">
                  <div className="spinner-border text-primary" role="status" />
                  <p className="mt-2 text-muted">Carregando...</p>
                </div>
              )}

              {error && (
                <div className="alert alert-danger">
                  Erro ao carregar os dados do cliente. Tente novamente.
                </div>
              )}

              {customer && !loading && (
                <div className="row g-3">
                  <div className="col-12 col-md-6">
                    <p className="text-muted small mb-0">Nome</p>
                    <p className="fw-semibold mb-0">{customer.name}</p>
                  </div>
                  <div className="col-12 col-md-6">
                    <p className="text-muted small mb-0">E-mail</p>
                    <p className="fw-semibold mb-0">{customer.email}</p>
                  </div>
                  <div className="col-12 col-md-6">
                    <p className="text-muted small mb-0">Telefone</p>
                    <p className="fw-semibold mb-0">{formatPhone(customer.phone)}</p>
                  </div>
                  <div className="col-12 col-md-6">
                    <p className="text-muted small mb-0">Interesse Principal</p>
                    <p className="fw-semibold mb-0">
                      {INTEREST_LABELS[(customer as any).mainInterest] ?? '—'}
                    </p>
                  </div>
                  <div className="col-12 col-md-6">
                    <p className="text-muted small mb-0">Cadastrado em</p>
                    <p className="fw-semibold mb-0">
                      {formatDate((customer as any).createDate ?? (customer as any).createdAt)}
                    </p>
                  </div>
                  <div className="col-12 col-md-6">
                    <p className="text-muted small mb-0">Última alteração</p>
                    <p className="fw-semibold mb-0">
                      {formatDate((customer as any).modificationDate ?? (customer as any).updatedAt)}
                    </p>
                  </div>
                </div>
              )}
            </div>

            <div className="modal-footer">
              <button type="button" className="btn btn-secondary" onClick={onClose}>
                Fechar
              </button>
            </div>
          </div>
        </div>
      </div>
    </>
  )
}
