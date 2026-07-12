import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { DataGrid } from '../../components/DataGrid'
import type { Column } from '../../components/DataGrid'
import type { Customer } from '../../types'

const INTEREST_LABELS: Record<number, string> = {
  1: 'SUV',
  2: 'Hatch',
  3: 'Sedan',
  4: 'Utilitário',
  5: 'Carro Usado',
  6: 'Carro Novo',
}

const FAKE_DATA: Customer[] = [
  {
    id: 1,
    name: 'Ana Souza',
    email: 'ana.souza@email.com',
    phone: '(11) 91234-5678',
    mainInterest: 1,
    createdAt: '2024-03-10T09:00:00Z',
    updatedAt: '2024-03-10T09:00:00Z',
  },
  {
    id: 2,
    name: 'Carlos Mendes',
    email: 'carlos.mendes@email.com',
    phone: '(21) 98765-4321',
    mainInterest: 3,
    createdAt: '2024-04-05T14:30:00Z',
    updatedAt: '2024-04-05T14:30:00Z',
  },
  {
    id: 3,
    name: 'Fernanda Lima',
    email: 'fernanda.lima@email.com',
    phone: '(31) 97654-3210',
    mainInterest: 6,
    createdAt: '2024-05-20T11:00:00Z',
    updatedAt: '2024-05-20T11:00:00Z',
  },
  {
    id: 4,
    name: 'Roberto Alves',
    email: 'roberto.alves@email.com',
    phone: '(41) 96543-2109',
    mainInterest: 2,
    createdAt: '2024-06-01T08:45:00Z',
    updatedAt: '2024-06-01T08:45:00Z',
  },
  {
    id: 5,
    name: 'Juliana Pereira',
    email: 'juliana.pereira@email.com',
    phone: '(51) 95432-1098',
    mainInterest: 5,
    createdAt: '2024-06-15T16:00:00Z',
    updatedAt: '2024-06-15T16:00:00Z',
  },
]

const COLUMNS: Column<Customer>[] = [
  { key: 'name', header: 'Nome', accessor: 'name' },
  { key: 'email', header: 'E-mail', accessor: 'email' },
  { key: 'phone', header: 'Telefone', accessor: 'phone' },
  {
    key: 'mainInterest',
    header: 'Interesse Principal',
    render: row => INTEREST_LABELS[row.mainInterest] ?? '—',
  },
]

export default function Customers() {
  const [search, setSearch] = useState('')
  const [page, setPage] = useState(1)
  const [pageSize, setPageSize] = useState(10)
  const navigate = useNavigate()

  const filtered = FAKE_DATA.filter(
    c =>
      search === '' ||
      c.name.toLowerCase().includes(search.toLowerCase()) ||
      c.email.toLowerCase().includes(search.toLowerCase()),
  )

  return (
    <div>
      <div className="d-flex align-items-center justify-content-between mb-4">
        <h1 className="h3 mb-0">Clientes</h1>
      </div>
      <div className="d-flex gap-2 mb-4">
        <input
          type="text"
          className="form-control"
          placeholder="Buscar por nome ou e-mail..."
          value={search}
          onChange={e => {
            setSearch(e.target.value)
            setPage(1)
          }}
        />
        <button
          className="btn btn-primary text-nowrap"
          onClick={() => navigate('/customers/new')}
        >
          <i className="bi bi-plus-lg me-1" />
          Novo Cliente
        </button>
      </div>

      <DataGrid<Customer>
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
