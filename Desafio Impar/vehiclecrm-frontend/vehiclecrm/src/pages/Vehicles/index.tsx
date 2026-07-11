import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { DataGrid } from '../../components/DataGrid'
import type { Column } from '../../components/DataGrid'
import type { Vehicle } from '../../types'

const STATUS_LABELS: Record<number, { label: string; className: string }> = {
  1: { label: 'Disponível', className: 'badge bg-success' },
  2: { label: 'Vendido', className: 'badge bg-secondary' },
  3: { label: 'Reservado', className: 'badge bg-warning text-dark' },
}

const FAKE_DATA: Vehicle[] = [
  {
    id: 1,
    brand: 'Toyota',
    model: 'Corolla',
    year: 2023,
    color: 'Prata',
    mileage: 12500,
    price: 125000,
    plate: 'ABC-1234',
    status: 1,
    createdAt: '2024-01-15T10:00:00Z',
    updatedAt: '2024-01-15T10:00:00Z',
  },
]

const COLUMNS: Column<Vehicle>[] = [
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
  { key: 'plate', header: 'Placa', render: row => row.plate ?? '—' },
  {
    key: 'status',
    header: 'Status',
    render: row => {
      const s = STATUS_LABELS[row.status]
      return <span className={s.className}>{s.label}</span>
    },
  },
]

export default function Vehicles() {
  const [search, setSearch] = useState('')
  const [page, setPage] = useState(1)
  const [pageSize, setPageSize] = useState(10)
  const navigate = useNavigate()

  // Filtered data (will be replaced by API call)
  const filtered = FAKE_DATA.filter(
    v =>
      search === '' ||
      v.brand.toLowerCase().includes(search.toLowerCase()) ||
      v.model.toLowerCase().includes(search.toLowerCase()),
  )

  return (
    <div>
      <div className="d-flex align-items-center justify-content-between mb-4">
        <h1 className="h3 mb-0">Veículos</h1>
      </div>
      <div className="d-flex gap-2 mb-4">
        <input
          type="text"
          className="form-control"
          placeholder="Buscar por marca ou modelo..."
          value={search}
          onChange={e => {
            setSearch(e.target.value)
            setPage(1)
          }}
        />
        <button
          className="btn btn-primary text-nowrap"
          onClick={() => navigate('/vehicles/new')}
        >
          <i className="bi bi-plus-lg me-1" />
          Novo Veículo
        </button>
      </div>

      <DataGrid<Vehicle>
        columns={COLUMNS}
        data={filtered}
        total={filtered.length}
        page={page}
        pageSize={pageSize}
        onPageChange={setPage}
        onPageSizeChange={size => { setPageSize(size); setPage(1) }}
        keyExtractor={row => row.id}
      />
    </div>
  )
}
