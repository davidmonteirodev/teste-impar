import { useState, useEffect, useCallback } from 'react'
import { useNavigate } from 'react-router-dom'
import Swal from 'sweetalert2'
import { DataGrid } from '../../components/DataGrid'
import type { Column } from '../../components/DataGrid'
import type { Customer, CustomerFilters } from '../../types'
import { customerService } from '../../services/customerService'
import CustomerDetailModal from './CustomerDetailModal'

const INTEREST_LABELS: Record<number, string> = {
  1: 'SUV',
  2: 'Hatch',
  3: 'Sedan',
  4: 'Utilitário',
  5: 'Carro Usado',
  6: 'Carro Novo',
}

function buildColumns(
  onView: (id: number) => void,
  onEdit: (id: number) => void,
  onDelete: (id: number) => void,
): Column<Customer>[] {
  return [
    { key: 'name', header: 'Nome', accessor: 'name' },
    { key: 'email', header: 'E-mail', accessor: 'email' },
    {
      key: 'mainInterest',
      header: 'Interesse Principal',
      render: row => INTEREST_LABELS[row.mainInterest] ?? '—',
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
            title="Editar"
            onClick={() => onEdit(row.id)}
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

const EMPTY_FILTERS: CustomerFilters = {
  name: '',
  email: '',
  phone: '',
  mainInterest: undefined,
}

export default function Customers() {
  const [filters, setFilters] = useState<CustomerFilters>(EMPTY_FILTERS)
  const [pendingFilters, setPendingFilters] = useState<CustomerFilters>(EMPTY_FILTERS)
  const [page, setPage] = useState(1)
  const [pageSize, setPageSize] = useState(10)
  const [customers, setCustomers] = useState<Customer[]>([])
  const [total, setTotal] = useState(0)
  const [loading, setLoading] = useState(false)
  const [detailCustomerId, setDetailCustomerId] = useState<number | null>(null)
  const navigate = useNavigate()

  const handleDelete = useCallback(async (id: number) => {
    const confirm = await Swal.fire({
      title: 'Excluir cliente',
      text: 'Tem certeza que deseja excluir este cliente? Esta ação não poderá ser desfeita.',
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
      await customerService.remove(id)
      await Swal.fire({
        title: 'Excluído!',
        text: 'O cliente foi excluído com sucesso.',
        icon: 'success',
        confirmButtonColor: '#0d6efd',
      })
      setFilters(f => ({ ...f }))
    } catch (err: any) {
      const message =
        err?.response?.data?.message ??
        err?.response?.data ??
        'Ocorreu um erro ao excluir o cliente. Tente novamente.'
      await Swal.fire({
        title: 'Erro ao excluir',
        text: typeof message === 'string' ? message : JSON.stringify(message),
        icon: 'error',
        confirmButtonColor: '#0d6efd',
      })
    }
  }, [])

  const columns = buildColumns(
    id => setDetailCustomerId(id),
    id => navigate(`/customers/${id}/edit`),
    handleDelete,
  )

  useEffect(() => {
    const controller = new AbortController()

    const fetch = async () => {
      setLoading(true)
      try {
        const res = await customerService.getCustomers({ ...filters, page, pageSize }, controller.signal)
        setCustomers(res.data.items)
        setTotal(res.data.totalItems)
      } catch {
        if (!controller.signal.aborted) {
          setCustomers([])
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
        <h1 className="h3 mb-0">Clientes</h1>
        <button
          className="btn btn-primary text-nowrap"
          onClick={() => navigate('/customers/new')}
        >
          <i className="bi bi-plus-lg me-1" />
          Novo Cliente
        </button>
      </div>

      {/* Filter panel */}
      <div className="card mb-4">
        <div className="card-body">
          <div className="row g-3">
            <div className="col-12 col-sm-6 col-md-4 col-lg-3">
              <label className="form-label small fw-semibold">Nome</label>
              <input
                type="text"
                className="form-control form-control-sm"
                placeholder="Ex: João Silva"
                value={pendingFilters.name ?? ''}
                onChange={e => setPendingFilters(f => ({ ...f, name: e.target.value }))}
              />
            </div>
            <div className="col-12 col-sm-6 col-md-4 col-lg-3">
              <label className="form-label small fw-semibold">E-mail</label>
              <input
                type="text"
                className="form-control form-control-sm"
                placeholder="Ex: joao@email.com"
                value={pendingFilters.email ?? ''}
                onChange={e => setPendingFilters(f => ({ ...f, email: e.target.value }))}
              />
            </div>
            <div className="col-12 col-sm-6 col-md-4 col-lg-3">
              <label className="form-label small fw-semibold">Telefone</label>
              <input
                type="text"
                className="form-control form-control-sm"
                placeholder="Ex: (11) 91234-5678"
                value={pendingFilters.phone ?? ''}
                onChange={e => setPendingFilters(f => ({ ...f, phone: e.target.value }))}
              />
            </div>
            <div className="col-12 col-sm-6 col-md-4 col-lg-3">
              <label className="form-label small fw-semibold">Interesse Principal</label>
              <select
                className="form-select form-select-sm"
                value={pendingFilters.mainInterest ?? ''}
                onChange={e =>
                  setPendingFilters(f => ({
                    ...f,
                    mainInterest: e.target.value ? Number(e.target.value) : undefined,
                  }))
                }
              >
                <option value="">Todos</option>
                <option value="1">SUV</option>
                <option value="2">Hatch</option>
                <option value="3">Sedan</option>
                <option value="4">Utilitário</option>
                <option value="5">Carro Usado</option>
                <option value="6">Carro Novo</option>
              </select>
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

      <DataGrid<Customer>
        columns={columns}
        data={customers}
        total={total}
        page={page}
        pageSize={pageSize}
        onPageChange={setPage}
        onPageSizeChange={handlePageSizeChange}
        loading={loading}
        keyExtractor={row => row.id}
      />

      <CustomerDetailModal
        customerId={detailCustomerId}
        onClose={() => setDetailCustomerId(null)}
      />
    </div>
  )
}
