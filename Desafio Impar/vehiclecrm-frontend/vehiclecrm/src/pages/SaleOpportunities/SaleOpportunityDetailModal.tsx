import { useEffect, useState } from 'react'
import { saleOpportunityService } from '../../services/saleOpportunityService'
import type { SaleOpportunity } from '../../types'

interface Props {
  opportunityId: number | null
  onClose: () => void
}

const STATUS_LABELS: Record<number, { label: string; className: string }> = {
  1: { label: 'Novo lead', className: 'badge bg-primary' },
  2: { label: 'Em negociação', className: 'badge bg-warning text-dark' },
  3: { label: 'Proposta enviada', className: 'badge bg-info text-dark' },
  4: { label: 'Vendido', className: 'badge bg-success' },
  5: { label: 'Perdido', className: 'badge bg-danger' },
}

function formatDate(value: string | null | undefined): string {
  if (!value) return '—'
  return new Date(value).toLocaleString('pt-BR')
}

export default function SaleOpportunityDetailModal({ opportunityId, onClose }: Props) {
  const [opportunity, setOpportunity] = useState<SaleOpportunity | null>(null)
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState(false)

  useEffect(() => {
    if (opportunityId === null) {
      setOpportunity(null)
      return
    }
    let cancelled = false
    setLoading(true)
    setError(false)
    saleOpportunityService
      .getById(opportunityId)
      .then(res => {
        if (!cancelled) setOpportunity(res.data)
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
  }, [opportunityId])

  if (opportunityId === null) return null

  const status = opportunity ? STATUS_LABELS[opportunity.status] : null

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
                <i className="bi bi-cash-coin me-2" />
                Detalhes da Oportunidade de Venda
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
                  Erro ao carregar os dados da oportunidade. Tente novamente.
                </div>
              )}

              {opportunity && !loading && (
                <div className="row g-3">
                  <div className="col-12 col-md-6">
                    <p className="text-muted small mb-0">Modelo do veículo</p>
                    <p className="fw-semibold mb-0">{opportunity.vehicle.model}</p>
                  </div>
                  <div className="col-12 col-md-6">
                    <p className="text-muted small mb-0">Nome do cliente</p>
                    <p className="fw-semibold mb-0">{opportunity.customer.name}</p>
                  </div>
                  <div className="col-12 col-md-6">
                    <p className="text-muted small mb-0">Valor proposto</p>
                    <p className="fw-semibold mb-0">
                      {opportunity.proposedValue.toLocaleString('pt-BR', {
                        style: 'currency',
                        currency: 'BRL',
                      })}
                    </p>
                  </div>
                  <div className="col-12 col-md-6">
                    <p className="text-muted small mb-0">Status</p>
                    <p className="fw-semibold mb-0">
                      {status ? (
                        <span className={status.className}>{status.label}</span>
                      ) : (
                        opportunity.status
                      )}
                    </p>
                  </div>
                  {opportunity.notes && (
                    <div className="col-12">
                      <p className="text-muted small mb-0">Observações</p>
                      <p className="fw-semibold mb-0">{opportunity.notes}</p>
                    </div>
                  )}
                  <div className="col-12 col-md-6">
                    <p className="text-muted small mb-0">Data de criação</p>
                    <p className="fw-semibold mb-0">{formatDate(opportunity.createDate)}</p>
                  </div>
                  <div className="col-12 col-md-6">
                    <p className="text-muted small mb-0">Última modificação</p>
                    <p className="fw-semibold mb-0">{formatDate(opportunity.modificationDate)}</p>
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
