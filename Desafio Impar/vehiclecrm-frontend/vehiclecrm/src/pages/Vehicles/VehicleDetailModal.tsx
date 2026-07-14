import { useEffect, useState } from 'react'
import { vehicleService } from '../../services/vehicleService'
import type { Vehicle } from '../../types'

interface Props {
  vehicleId: number | null
  onClose: () => void
}

const STATUS_LABELS: Record<number, { label: string; className: string }> = {
  1: { label: 'Disponível', className: 'badge bg-success' },
  2: { label: 'Reservado', className: 'badge bg-warning text-dark' },
  3: { label: 'Vendido', className: 'badge bg-secondary' },
}

function formatDate(value: string | null | undefined): string {
  if (!value) return '—'
  return new Date(value).toLocaleString('pt-BR')
}

export default function VehicleDetailModal({ vehicleId, onClose }: Props) {
  const [vehicle, setVehicle] = useState<Vehicle | null>(null)
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState(false)

  useEffect(() => {
    if (vehicleId === null) {
      setVehicle(null)
      return
    }
    let cancelled = false
    setLoading(true)
    setError(false)
    vehicleService
      .getById(vehicleId)
      .then(res => {
        if (!cancelled) setVehicle(res.data)
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
  }, [vehicleId])

  if (vehicleId === null) return null

  const status = vehicle ? STATUS_LABELS[vehicle.status] : null

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
                <i className="bi bi-car-front me-2" />
                Detalhes do Veículo
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
                  Erro ao carregar os dados do veículo. Tente novamente.
                </div>
              )}

              {vehicle && !loading && (
                <div className="row g-3">
                  <div className="col-6 col-md-4">
                    <p className="text-muted small mb-0">Marca</p>
                    <p className="fw-semibold mb-0">{vehicle.brand}</p>
                  </div>
                  <div className="col-6 col-md-4">
                    <p className="text-muted small mb-0">Modelo</p>
                    <p className="fw-semibold mb-0">{vehicle.model}</p>
                  </div>
                  <div className="col-6 col-md-4">
                    <p className="text-muted small mb-0">Ano</p>
                    <p className="fw-semibold mb-0">{vehicle.year}</p>
                  </div>
                  <div className="col-6 col-md-4">
                    <p className="text-muted small mb-0">Cor</p>
                    <p className="fw-semibold mb-0">{vehicle.color}</p>
                  </div>
                  <div className="col-6 col-md-4">
                    <p className="text-muted small mb-0">Quilometragem</p>
                    <p className="fw-semibold mb-0">
                      {vehicle.mileage.toLocaleString('pt-BR')} km
                    </p>
                  </div>
                  <div className="col-6 col-md-4">
                    <p className="text-muted small mb-0">Preço</p>
                    <p className="fw-semibold mb-0">
                      {vehicle.price.toLocaleString('pt-BR', {
                        style: 'currency',
                        currency: 'BRL',
                      })}
                    </p>
                  </div>
                  <div className="col-6 col-md-4">
                    <p className="text-muted small mb-0">Status</p>
                    <p className="mb-0">
                      <span className={status?.className ?? 'badge bg-light text-dark'}>
                        {status?.label ?? vehicle.status}
                      </span>
                    </p>
                  </div>
                  <div className="col-6 col-md-4">
                    <p className="text-muted small mb-0">Cadastrado em</p>
                    <p className="fw-semibold mb-0">
                      {formatDate((vehicle as any).createDate ?? (vehicle as any).createdAt)}
                    </p>
                  </div>
                  <div className="col-6 col-md-4">
                    <p className="text-muted small mb-0">Última alteração</p>
                    <p className="fw-semibold mb-0">
                      {formatDate((vehicle as any).modificationDate ?? (vehicle as any).updatedAt)}
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
