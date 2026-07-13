import { useState, useEffect, useCallback } from 'react'
import { useNavigate } from 'react-router-dom'
import Swal from 'sweetalert2'
import { DataGrid } from '../../components/DataGrid'
import type { Column } from '../../components/DataGrid'
import type { Vehicle } from '../../types'
import { vehicleService } from '../../services/vehicleService'
import type { VehicleFilters } from '../../types'
import VehicleDetailModal from './VehicleDetailModal'

const STATUS_LABELS: Record<number, { label: string; className: string }> = {
  1: { label: 'Disponível', className: 'badge bg-success' },
  2: { label: 'Reservado', className: 'badge bg-warning text-dark' },
  3: { label: 'Vendido', className: 'badge bg-secondary' },
}

function buildColumns(
  onView: (id: number) => void,
  onEdit: (id: number) => void,
  onDelete: (id: number) => void,
): Column<Vehicle>[] {
  return [
    { key: 'brand', header: 'Marca', accessor: 'brand' },
    { key: 'model', header: 'Modelo', accessor: 'model' },
    { key: 'year', header: 'Ano', accessor: 'year' },
    { key: 'color', header: 'Cor', accessor: 'color' },
    {
      key: 'mileage',
      header: 'Km',
      render: row => row.mileage.toLocaleString('pt-BR') + ' km',
    },
    {
      key: 'price',
      header: 'Preço',
      render: row =>
        row.price.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' }),
    },
    {
      key: 'status',
      header: 'Status',
      render: row => {
        const s = STATUS_LABELS[row.status]
        return <span className={s?.className ?? 'badge bg-light text-dark'}>{s?.label ?? row.status}</span>
      },
    },
    {
      key: 'actions',
      header: 'Ações',
      render: row => (
        <div className="d-flex gap-1">
          <button
            className="btn btn-sm btn-outline-info"
            title="Visualizar detalhes"
            onClick={() => onView(row.id)}
          >
            <i className="bi bi-eye" />
          </button>
          <button
            className="btn btn-sm btn-outline-primary"
            title={row.status === 3 ? 'Veículo vendido não pode ser editado' : row.status === 2 ? 'Veículo reservado não pode ser editado' : 'Editar'}
            onClick={() => onEdit(row.id)}
            disabled={row.status === 2 || row.status === 3}
          >
            <i className="bi bi-pencil" />
          </button>
          <button
            className="btn btn-sm btn-outline-danger"
            title="Excluir"
            onClick={() => onDelete(row.id)}
          >
            <i className="bi bi-trash" />
          </button>
        </div>
      ),
    },
  ]
}

const EMPTY_FILTERS: VehicleFilters = {
  brand: '',
  model: '',
  color: '',
  yearFrom: undefined,
  yearTo: undefined,
  priceFrom: undefined,
  priceTo: undefined,
  mileageFrom: undefined,
  mileageTo: undefined,
}

export default function Vehicles() {
  const [filters, setFilters] = useState<VehicleFilters>(EMPTY_FILTERS)
  const [pendingFilters, setPendingFilters] = useState<VehicleFilters>(EMPTY_FILTERS)
  const [page, setPage] = useState(1)
  const [pageSize, setPageSize] = useState(10)
  const [vehicles, setVehicles] = useState<Vehicle[]>([])
  const [total, setTotal] = useState(0)
  const [loading, setLoading] = useState(false)
  const [detailVehicleId, setDetailVehicleId] = useState<number | null>(null)
  const navigate = useNavigate()

  const handleDelete = useCallback(async (id: number) => {
    const confirm = await Swal.fire({
      title: 'Excluir veículo',
      text: 'Tem certeza que deseja excluir este veículo? Esta ação não poderá ser desfeita.',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#dc3545',
      cancelButtonColor: '#6c757d',
      confirmButtonText: 'Sim, excluir',
      cancelButtonText: 'Cancelar',
    })

    if (!confirm.isConfirmed) return

    Swal.fire({
      title: 'Excluindo...',
      allowOutsideClick: false,
      allowEscapeKey: false,
      didOpen: () => Swal.showLoading(),
    })

    try {
      await vehicleService.remove(id)
      await Swal.fire({
        title: 'Excluído!',
        text: 'O veículo foi excluído com sucesso.',
        icon: 'success',
        confirmButtonColor: '#0d6efd',
      })
      setFilters(f => ({ ...f }))
    } catch (err: any) {
      const message =
        err?.response?.data?.message ??
        err?.response?.data ??
        'Ocorreu um erro ao excluir o veículo. Tente novamente.'
      await Swal.fire({
        title: 'Erro ao excluir',
        text: typeof message === 'string' ? message : JSON.stringify(message),
        icon: 'error',
        confirmButtonColor: '#0d6efd',
      })
    }
  }, [])

  const columns = buildColumns(
    id => setDetailVehicleId(id),
    id => navigate(`/vehicles/${id}/edit`),
    handleDelete,
  )

  useEffect(() => {
    const controller = new AbortController()

    const fetch = async () => {
      setLoading(true)
      try {
        const res = await vehicleService.getVehicles({ ...filters, page, pageSize }, controller.signal)
        setVehicles(res.data.items)
        setTotal(res.data.totalItems)
      } catch {
        if (!controller.signal.aborted) {
          setVehicles([])
          setTotal(0)
        }
      } finally {
        if (!controller.signal.aborted) {
          setLoading(false)
        }
      }
    }

    fetch()
    return () => controller.abort()
  }, [filters, page, pageSize])

  const handleSearch = () => {
    setFilters(pendingFilters)
    setPage(1)
    setPageSize(10)
  }

  const handleClear = () => {
    setPendingFilters(EMPTY_FILTERS)
  }

  const handlePageSizeChange = (size: number) => {
    setPageSize(size)
    setPage(1)
  }

  return (
    <div>
      <div className="d-flex align-items-center justify-content-between mb-3 flex-wrap gap-2">
        <h1 className="h3 mb-0">Veículos</h1>
        <button
          className="btn btn-primary text-nowrap"
          onClick={() => navigate('/vehicles/new')}
        >
          <i className="bi bi-plus-lg me-1" />
          Novo Veículo
        </button>
      </div>

      {/* Filter panel */}
      <div className="card mb-4">
        <div className="card-body">
          <div className="row g-3">
            <div className="col-12 col-sm-6 col-md-4 col-lg-3">
              <label className="form-label small fw-semibold">Marca</label>
              <input
                type="text"
                className="form-control form-control-sm"
                placeholder="Ex: Toyota"
                value={pendingFilters.brand ?? ''}
                onChange={e => setPendingFilters(f => ({ ...f, brand: e.target.value }))}
              />
            </div>
            <div className="col-12 col-sm-6 col-md-4 col-lg-3">
              <label className="form-label small fw-semibold">Modelo</label>
              <input
                type="text"
                className="form-control form-control-sm"
                placeholder="Ex: Corolla"
                value={pendingFilters.model ?? ''}
                onChange={e => setPendingFilters(f => ({ ...f, model: e.target.value }))}
              />
            </div>
            <div className="col-12 col-sm-6 col-md-4 col-lg-3">
              <label className="form-label small fw-semibold">Cor</label>
              <input
                type="text"
                className="form-control form-control-sm"
                placeholder="Ex: Prata"
                value={pendingFilters.color ?? ''}
                onChange={e => setPendingFilters(f => ({ ...f, color: e.target.value }))}
              />
            </div>
            <div className="col-6 col-sm-3 col-md-2 col-lg-2">
              <label className="form-label small fw-semibold">Ano de</label>
              <input
                type="number"
                className="form-control form-control-sm"
                placeholder="2000"
                value={pendingFilters.yearFrom ?? ''}
                onChange={e =>
                  setPendingFilters(f => ({ ...f, yearFrom: e.target.value ? Number(e.target.value) : undefined }))
                }
              />
            </div>
            <div className="col-6 col-sm-3 col-md-2 col-lg-2">
              <label className="form-label small fw-semibold">Ano até</label>
              <input
                type="number"
                className="form-control form-control-sm"
                placeholder="2026"
                value={pendingFilters.yearTo ?? ''}
                onChange={e =>
                  setPendingFilters(f => ({ ...f, yearTo: e.target.value ? Number(e.target.value) : undefined }))
                }
              />
            </div>
            <div className="col-6 col-sm-3 col-md-2 col-lg-2">
              <label className="form-label small fw-semibold">Preço de (R$)</label>
              <input
                type="number"
                className="form-control form-control-sm"
                placeholder="0"
                value={pendingFilters.priceFrom ?? ''}
                onChange={e =>
                  setPendingFilters(f => ({ ...f, priceFrom: e.target.value ? Number(e.target.value) : undefined }))
                }
              />
            </div>
            <div className="col-6 col-sm-3 col-md-2 col-lg-2">
              <label className="form-label small fw-semibold">Preço até (R$)</label>
              <input
                type="number"
                className="form-control form-control-sm"
                placeholder="999999"
                value={pendingFilters.priceTo ?? ''}
                onChange={e =>
                  setPendingFilters(f => ({ ...f, priceTo: e.target.value ? Number(e.target.value) : undefined }))
                }
              />
            </div>
            <div className="col-6 col-sm-3 col-md-2 col-lg-2">
              <label className="form-label small fw-semibold">KM de</label>
              <input
                type="number"
                className="form-control form-control-sm"
                placeholder="0"
                value={pendingFilters.mileageFrom ?? ''}
                onChange={e =>
                  setPendingFilters(f => ({ ...f, mileageFrom: e.target.value ? Number(e.target.value) : undefined }))
                }
              />
            </div>
            <div className="col-6 col-sm-3 col-md-2 col-lg-2">
              <label className="form-label small fw-semibold">KM até</label>
              <input
                type="number"
                className="form-control form-control-sm"
                placeholder="999999"
                value={pendingFilters.mileageTo ?? ''}
                onChange={e =>
                  setPendingFilters(f => ({ ...f, mileageTo: e.target.value ? Number(e.target.value) : undefined }))
                }
              />
            </div>
            <div className="col-12 d-flex gap-2 justify-content-end">
              <button className="btn btn-outline-secondary btn-sm" onClick={handleClear}>
                <i className="bi bi-x-circle me-1" />
                Limpar
              </button>
              <button className="btn btn-primary btn-sm" onClick={handleSearch}>
                <i className="bi bi-search me-1" />
                Buscar
              </button>
            </div>
          </div>
        </div>
      </div>

      <DataGrid<Vehicle>
        columns={columns}
        data={vehicles}
        total={total}
        page={page}
        pageSize={pageSize}
        onPageChange={setPage}
        onPageSizeChange={handlePageSizeChange}
        loading={loading}
        keyExtractor={row => row.id}
      />

      <VehicleDetailModal
        vehicleId={detailVehicleId}
        onClose={() => setDetailVehicleId(null)}
      />
    </div>
  )
}

