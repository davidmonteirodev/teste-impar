import { useState, useEffect, useCallback } from 'react'
import { useNavigate } from 'react-router-dom'
import Swal from 'sweetalert2'
import { DataGrid } from '../../components/DataGrid'
import type { Column } from '../../components/DataGrid'
import type { SaleOpportunity, SaleOpportunityFilters, SaleOpportunityStatus } from '../../types'
import { saleOpportunityService } from '../../services/saleOpportunityService'
import SaleOpportunityDetailModal from './SaleOpportunityDetailModal'

const STATUS_LABELS: Record<number, { label: string; className: string }> = {
  1: { label: 'Novo lead', className: 'badge bg-primary' },
  2: { label: 'Em negociação', className: 'badge bg-warning text-dark' },
  3: { label: 'Proposta enviada', className: 'badge bg-info text-dark' },
  4: { label: 'Vendido', className: 'badge bg-success' },
  5: { label: 'Perdido', className: 'badge bg-danger' },
}

const STATUS_OPTIONS: { value: SaleOpportunityStatus; label: string }[] = [
  { value: 1, label: 'Novo lead' },
  { value: 2, label: 'Em negociação' },
  { value: 3, label: 'Proposta enviada' },
  { value: 4, label: 'Vendido' },
  { value: 5, label: 'Perdido' },
]

function buildColumns(
  onView: (id: number) => void,
  onEdit: (id: number) => void,
  onDelete: (id: number) => void,
): Column<SaleOpportunity>[] {
  return [
    { key: 'vehicleModel', header: 'Modelo do veículo', render: row => row.vehicle.model },
    { key: 'customerName', header: 'Nome do cliente', render: row => row.customer.name },
    {
      key: 'proposedValue',
      header: 'Valor proposto',
      render: row =>
        row.proposedValue.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' }),
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
            title={row.status === 4 ? 'Oportunidade vendida não pode ser editada' : 'Editar'}
            onClick={() => onEdit(row.id)}
            disabled={row.status === 4}
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

const EMPTY_FILTERS: SaleOpportunityFilters = {
  vehicleModel: '',
  customerName: '',
  proposedValueFrom: undefined,
  proposedValueTo: undefined,
  status: undefined,
}

export default function SaleOpportunities() {
  const [filters, setFilters] = useState<SaleOpportunityFilters>(EMPTY_FILTERS)
  const [pendingFilters, setPendingFilters] = useState<SaleOpportunityFilters>(EMPTY_FILTERS)
  const [page, setPage] = useState(1)
  const [pageSize, setPageSize] = useState(10)
  const [opportunities, setOpportunities] = useState<SaleOpportunity[]>([])
  const [total, setTotal] = useState(0)
  const [loading, setLoading] = useState(false)
  const [detailOpportunityId, setDetailOpportunityId] = useState<number | null>(null)
  const navigate = useNavigate()

  const handleDelete = useCallback(async (id: number) => {
    const confirm = await Swal.fire({
      title: 'Excluir oportunidade',
      text: 'Tem certeza que deseja excluir esta oportunidade de venda? Esta ação não poderá ser desfeita.',
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
      await saleOpportunityService.remove(id)
      await Swal.fire({
        title: 'Excluída!',
        text: 'A oportunidade de venda foi excluída com sucesso.',
        icon: 'success',
        confirmButtonColor: '#0d6efd',
      })
      setFilters(f => ({ ...f }))
    } catch (err: any) {
      const message =
        err?.response?.data?.message ??
        err?.response?.data ??
        'Ocorreu um erro ao excluir a oportunidade. Tente novamente.'
      await Swal.fire({
        title: 'Erro ao excluir',
        text: typeof message === 'string' ? message : JSON.stringify(message),
        icon: 'error',
        confirmButtonColor: '#0d6efd',
      })
    }
  }, [])

  const columns = buildColumns(
    id => setDetailOpportunityId(id),
    id => navigate(`/sale-opportunities/${id}/edit`),
    handleDelete,
  )

  useEffect(() => {
    const controller = new AbortController()

    const fetch = async () => {
      setLoading(true)
      try {
        const res = await saleOpportunityService.getSaleOpportunities(
          { ...filters, page, pageSize },
          controller.signal,
        )
        setOpportunities(res.data.items)
        setTotal(res.data.totalItems)
      } catch {
        if (!controller.signal.aborted) {
          setOpportunities([])
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
        <h1 className="h3 mb-0">Oportunidades de vendas</h1>
        <button
          className="btn btn-primary text-nowrap"
          onClick={() => navigate('/sale-opportunities/new')}
        >
          <i className="bi bi-plus-lg me-1" />
          Nova oportunidade
        </button>
      </div>

      {/* Filter panel */}
      <div className="card mb-4">
        <div className="card-body">
          <div className="row g-3">
            <div className="col-12 col-sm-6 col-md-4 col-lg-3">
              <label className="form-label small fw-semibold">Modelo do veículo</label>
              <input
                type="text"
                className="form-control form-control-sm"
                placeholder="Ex: Corolla"
                value={pendingFilters.vehicleModel ?? ''}
                onChange={e => setPendingFilters(f => ({ ...f, vehicleModel: e.target.value }))}
              />
            </div>
            <div className="col-12 col-sm-6 col-md-4 col-lg-3">
              <label className="form-label small fw-semibold">Nome do cliente</label>
              <input
                type="text"
                className="form-control form-control-sm"
                placeholder="Ex: João Silva"
                value={pendingFilters.customerName ?? ''}
                onChange={e => setPendingFilters(f => ({ ...f, customerName: e.target.value }))}
              />
            </div>
            <div className="col-12 col-sm-6 col-md-4 col-lg-3">
              <label className="form-label small fw-semibold">Status da oportunidade</label>
              <select
                className="form-select form-select-sm"
                value={pendingFilters.status ?? ''}
                onChange={e =>
                  setPendingFilters(f => ({
                    ...f,
                    status: e.target.value ? (Number(e.target.value) as SaleOpportunityStatus) : undefined,
                  }))
                }
              >
                <option value="">Todos</option>
                {STATUS_OPTIONS.map(opt => (
                  <option key={opt.value} value={opt.value}>
                    {opt.label}
                  </option>
                ))}
              </select>
            </div>
            <div className="col-6 col-sm-3 col-md-2 col-lg-2">
              <label className="form-label small fw-semibold">Valor proposto de (R$)</label>
              <input
                type="number"
                className="form-control form-control-sm"
                placeholder="0"
                value={pendingFilters.proposedValueFrom ?? ''}
                onChange={e =>
                  setPendingFilters(f => ({
                    ...f,
                    proposedValueFrom: e.target.value ? Number(e.target.value) : undefined,
                  }))
                }
              />
            </div>
            <div className="col-6 col-sm-3 col-md-2 col-lg-2">
              <label className="form-label small fw-semibold">Valor proposto até (R$)</label>
              <input
                type="number"
                className="form-control form-control-sm"
                placeholder="999999"
                value={pendingFilters.proposedValueTo ?? ''}
                onChange={e =>
                  setPendingFilters(f => ({
                    ...f,
                    proposedValueTo: e.target.value ? Number(e.target.value) : undefined,
                  }))
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

      <DataGrid<SaleOpportunity>
        columns={columns}
        data={opportunities}
        total={total}
        page={page}
        pageSize={pageSize}
        onPageChange={setPage}
        onPageSizeChange={handlePageSizeChange}
        loading={loading}
        keyExtractor={row => row.id}
      />

      <SaleOpportunityDetailModal
        opportunityId={detailOpportunityId}
        onClose={() => setDetailOpportunityId(null)}
      />
    </div>
  )
}
