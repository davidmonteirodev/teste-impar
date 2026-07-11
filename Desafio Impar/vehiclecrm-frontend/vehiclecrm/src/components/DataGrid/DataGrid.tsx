import type { ReactNode } from 'react'

export interface Column<T> {
  key: string
  header: string
  accessor?: keyof T
  render?: (row: T) => ReactNode
}

interface DataGridProps<T> {
  columns: Column<T>[]
  data: T[]
  total: number
  page: number
  pageSize: number
  onPageChange: (page: number) => void
  onPageSizeChange?: (pageSize: number) => void
  loading?: boolean
  keyExtractor: (row: T) => string | number
  emptyMessage?: string
}

const PAGE_SIZE_OPTIONS = [5, 10, 25, 50]

export function DataGrid<T>({
  columns,
  data,
  total,
  page,
  pageSize,
  onPageChange,
  onPageSizeChange,
  loading = false,
  keyExtractor,
  emptyMessage = 'Nenhum registro encontrado.',
}: DataGridProps<T>) {
  const totalPages = Math.max(1, Math.ceil(total / pageSize))
  const from = total === 0 ? 0 : (page - 1) * pageSize + 1
  const to = Math.min(page * pageSize, total)

  const renderCell = (row: T, col: Column<T>): ReactNode => {
    if (col.render) return col.render(row)
    if (col.accessor != null) {
      const val = row[col.accessor]
      return val as ReactNode
    }
    return null
  }

  return (
    <div>
      <div className="table-responsive">
        <table className="table table-hover align-middle mb-0">
          <thead className="table-light">
            <tr>
              {columns.map(col => (
                <th key={col.key} scope="col">
                  {col.header}
                </th>
              ))}
            </tr>
          </thead>
          <tbody>
            {loading ? (
              <tr>
                <td colSpan={columns.length} className="text-center py-4">
                  <div className="spinner-border spinner-border-sm text-primary me-2" role="status" />
                  Carregando...
                </td>
              </tr>
            ) : data.length === 0 ? (
              <tr>
                <td colSpan={columns.length} className="text-center text-muted py-4">
                  {emptyMessage}
                </td>
              </tr>
            ) : (
              data.map(row => (
                <tr key={keyExtractor(row)}>
                  {columns.map(col => (
                    <td key={col.key}>{renderCell(row, col)}</td>
                  ))}
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      <div className="d-flex align-items-center justify-content-between mt-3 flex-wrap gap-2">
        <div className="d-flex align-items-center gap-2">
          {onPageSizeChange && (
            <>
              <span className="text-muted small">Linhas por página:</span>
              <select
                className="form-select form-select-sm"
                style={{ width: 'auto' }}
                value={pageSize}
                onChange={e => {
                  onPageSizeChange(Number(e.target.value))
                  onPageChange(1)
                }}
              >
                {PAGE_SIZE_OPTIONS.map(opt => (
                  <option key={opt} value={opt}>
                    {opt}
                  </option>
                ))}
              </select>
            </>
          )}
          <span className="text-muted small">
            {total === 0 ? '0 registros' : `${from}–${to} de ${total}`}
          </span>
        </div>

        <nav aria-label="Paginação">
          <ul className="pagination pagination-sm mb-0">
            <li className={`page-item${page <= 1 ? ' disabled' : ''}`}>
              <button
                className="page-link"
                onClick={() => onPageChange(1)}
                disabled={page <= 1}
                aria-label="Primeira página"
              >
                «
              </button>
            </li>
            <li className={`page-item${page <= 1 ? ' disabled' : ''}`}>
              <button
                className="page-link"
                onClick={() => onPageChange(page - 1)}
                disabled={page <= 1}
                aria-label="Página anterior"
              >
                ‹
              </button>
            </li>

            {Array.from({ length: totalPages }, (_, i) => i + 1)
              .filter(p => p === 1 || p === totalPages || Math.abs(p - page) <= 1)
              .reduce<(number | 'ellipsis-start' | 'ellipsis-end')[]>((acc, p, idx, arr) => {
                if (idx > 0 && (p as number) - (arr[idx - 1] as number) > 1) {
                  acc.push(p === totalPages ? 'ellipsis-end' : 'ellipsis-start')
                }
                acc.push(p)
                return acc
              }, [])
              .map((item, idx) =>
                typeof item === 'string' ? (
                  <li key={item + idx} className="page-item disabled">
                    <span className="page-link">…</span>
                  </li>
                ) : (
                  <li key={item} className={`page-item${item === page ? ' active' : ''}`}>
                    <button
                      className="page-link"
                      onClick={() => onPageChange(item)}
                      aria-current={item === page ? 'page' : undefined}
                    >
                      {item}
                    </button>
                  </li>
                )
              )}

            <li className={`page-item${page >= totalPages ? ' disabled' : ''}`}>
              <button
                className="page-link"
                onClick={() => onPageChange(page + 1)}
                disabled={page >= totalPages}
                aria-label="Próxima página"
              >
                ›
              </button>
            </li>
            <li className={`page-item${page >= totalPages ? ' disabled' : ''}`}>
              <button
                className="page-link"
                onClick={() => onPageChange(totalPages)}
                disabled={page >= totalPages}
                aria-label="Última página"
              >
                »
              </button>
            </li>
          </ul>
        </nav>
      </div>
    </div>
  )
}
