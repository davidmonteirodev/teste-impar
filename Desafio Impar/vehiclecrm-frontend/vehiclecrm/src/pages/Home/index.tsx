import { useEffect, useState } from 'react'
import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
  Cell,
  PieChart,
  Pie,
  Legend,
} from 'recharts'
import { getDashboard, type DashboardData } from '../../services/dashboardService'

const OPPORTUNITY_COLORS: Record<string, string> = {
  Lead: '#0d6efd',
  Negociação: '#fd7e14',
  Proposta: '#6f42c1',
  Vendido: '#198754',
  Perdido: '#dc3545',
}

const VEHICLE_COLORS: Record<string, string> = {
  Disponível: '#198754',
  Reservado: '#fd7e14',
  Vendido: '#0d6efd',
}

function getTodayLabel(): string {
  return new Date().toLocaleDateString('pt-BR', {
    weekday: 'long',
    day: 'numeric',
    month: 'short',
    year: 'numeric',
  })
}

function capitalize(s: string) {
  return s.charAt(0).toUpperCase() + s.slice(1)
}

export default function Home() {
  const [data, setData] = useState<DashboardData | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    getDashboard()
      .then(setData)
      .catch(() => setError('Não foi possível carregar o dashboard.'))
      .finally(() => setLoading(false))
  }, [])

  if (loading) {
    return (
      <div className="d-flex justify-content-center align-items-center" style={{ minHeight: 300 }}>
        <div className="spinner-border text-primary" role="status">
          <span className="visually-hidden">Carregando...</span>
        </div>
      </div>
    )
  }

  if (error || !data) {
    return (
      <div className="alert alert-danger" role="alert">
        {error ?? 'Erro ao carregar dados.'}
      </div>
    )
  }

  const { cards, vehicleStatus, opportunityStatus } = data

  const statCards = [
    { label: 'Veículos', value: cards.vehicles, icon: 'bi-car-front-fill', color: 'primary' },
    { label: 'Clientes', value: cards.customers, icon: 'bi-people-fill', color: 'success' },
    { label: 'Oportunidades', value: cards.opportunities, icon: 'bi-cash-coin', color: 'warning' },
    { label: 'Valor total vendido', value: cards.soldVehiclesTotalValue.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' }), icon: 'bi-currency-dollar', color: 'info' },
  ]

  const maxOpp = Math.max(...opportunityStatus.map(o => o.count), 1)

  return (
    <div>
      {/* Header */}
      <div className="d-flex justify-content-between align-items-center mb-4">
        <div>
          <h4 className="mb-0 fw-bold">Bem-vindo, <span className="text-primary">Ímpar</span></h4>
          <small className="text-muted">{capitalize(getTodayLabel())}</small>
        </div>
      </div>

      {/* Stat Cards */}
      <div className="row g-3 mb-4">
        {statCards.map(card => (
          <div key={card.label} className="col-6 col-md-3">
            <div className={`card border-0 shadow-sm h-100`}>
              <div className="card-body d-flex align-items-center gap-3">
                <div className={`rounded-circle bg-${card.color} bg-opacity-10 d-flex align-items-center justify-content-center`}
                  style={{ width: 52, height: 52, flexShrink: 0 }}>
                  <i className={`bi ${card.icon} fs-4 text-${card.color}`}></i>
                </div>
                <div style={{ minWidth: 0 }}>
                  <div className="fw-bold lh-1" style={{ fontSize: 'clamp(0.95rem, 2.5vw, 1.5rem)', wordBreak: 'break-word', overflowWrap: 'break-word' }}>{card.value}</div>
                  <div className="text-muted small">{card.label}</div>
                </div>
              </div>
            </div>
          </div>
        ))}
      </div>

      <div className="row g-4">
        {/* Oportunidades por Status — bar chart horizontal */}
        <div className="col-12 col-lg-7">
          <div className="card border-0 shadow-sm h-100">
            <div className="card-body">
              <h6 className="card-title fw-semibold mb-3">Oportunidades por Status</h6>

              {/* Custom horizontal bar list */}
              <div className="d-flex flex-column gap-2">
                {opportunityStatus.map(item => (
                  <div key={item.status}>
                    <div className="d-flex justify-content-between mb-1">
                      <span className="small">{item.status}</span>
                      <span className="small fw-semibold">{item.count}</span>
                    </div>
                    <div className="progress" style={{ height: 12, borderRadius: 6 }}>
                      <div
                        className="progress-bar"
                        role="progressbar"
                        style={{
                          width: `${(item.count / maxOpp) * 100}%`,
                          backgroundColor: OPPORTUNITY_COLORS[item.status] ?? '#6c757d',
                          borderRadius: 6,
                          transition: 'width 0.6s ease',
                        }}
                        aria-valuenow={item.count}
                        aria-valuemin={0}
                        aria-valuemax={maxOpp}
                      />
                    </div>
                  </div>
                ))}
              </div>

              {/* Recharts bar (vertical) */}
              <div className="mt-4" style={{ height: 200 }}>
                <ResponsiveContainer width="100%" height="100%">
                  <BarChart data={opportunityStatus} margin={{ top: 0, right: 10, left: -20, bottom: 0 }}>
                    <CartesianGrid strokeDasharray="3 3" vertical={false} />
                    <XAxis dataKey="status" tick={{ fontSize: 11 }} />
                    <YAxis allowDecimals={false} tick={{ fontSize: 11 }} />
                    <Tooltip />
                    <Bar dataKey="count" name="Quantidade" radius={[4, 4, 0, 0]}>
                      {opportunityStatus.map(entry => (
                        <Cell
                          key={entry.status}
                          fill={OPPORTUNITY_COLORS[entry.status] ?? '#6c757d'}
                        />
                      ))}
                    </Bar>
                  </BarChart>
                </ResponsiveContainer>
              </div>
            </div>
          </div>
        </div>

        {/* Veículos por Status — pie chart + table */}
        <div className="col-12 col-lg-5">
          <div className="card border-0 shadow-sm h-100">
            <div className="card-body">
              <h6 className="card-title fw-semibold mb-3">Veículos por Status</h6>

              <div style={{ height: 220 }}>
                <ResponsiveContainer width="100%" height="100%">
                  <PieChart>
                    <Pie
                      data={vehicleStatus}
                      dataKey="count"
                      nameKey="status"
                      cx="50%"
                      cy="50%"
                      outerRadius={80}
                      label={({ cx, cy, midAngle, innerRadius, outerRadius, percent }) => {
                        if (percent < 0.05) return null
                        const RADIAN = Math.PI / 180
                        const radius = innerRadius + (outerRadius - innerRadius) * 0.5
                        const x = cx + radius * Math.cos(-midAngle * RADIAN)
                        const y = cy + radius * Math.sin(-midAngle * RADIAN)
                        return (
                          <text x={x} y={y} fill="white" textAnchor="middle" dominantBaseline="central" fontSize={12} fontWeight="bold">
                            {`${(percent * 100).toFixed(0)}%`}
                          </text>
                        )
                      }}
                      labelLine={false}
                    >
                      {vehicleStatus.map(entry => (
                        <Cell
                          key={entry.status}
                          fill={VEHICLE_COLORS[entry.status] ?? '#adb5bd'}
                        />
                      ))}
                    </Pie>
                    <Legend iconType="circle" iconSize={10} />
                    <Tooltip formatter={(value, name) => [value, name]} />
                  </PieChart>
                </ResponsiveContainer>
              </div>

              <table className="table table-sm mt-2 mb-0">
                <tbody>
                  {vehicleStatus.map(item => (
                    <tr key={item.status}>
                      <td>
                        <span
                          className="d-inline-block rounded-circle me-2"
                          style={{
                            width: 10,
                            height: 10,
                            backgroundColor: VEHICLE_COLORS[item.status] ?? '#adb5bd',
                          }}
                        />
                        {item.status}
                      </td>
                      <td className="text-end fw-semibold">{item.count}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}
