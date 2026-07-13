import { NavLink } from 'react-router-dom'

const menuItems = [
  { to: '/', label: 'Home', icon: 'bi-house-fill', end: true },
  { to: '/vehicles', label: 'Veículos', icon: 'bi-car-front-fill', end: false },
  { to: '/customers', label: 'Clientes', icon: 'bi-people-fill', end: false },
  { to: '/sale-opportunities', label: 'Oportunidades de vendas', icon: 'bi-cash-coin', end: false },
]

interface SidebarProps {
  collapsed: boolean
  onToggle: () => void
}

export default function Sidebar({ collapsed, onToggle }: SidebarProps) {
  const handleNavClick = () => {
    // Fecha o drawer no mobile após navegar
    if (window.innerWidth < 768 && !collapsed) {
      onToggle()
    }
  }

  return (
    <aside className={`sidebar bg-white border-end d-flex flex-column py-3${collapsed ? ' collapsed' : ''}`}>
      {/* Botão de recolher/expandir */}
      <div className={`d-none d-md-flex mb-2 px-2 ${collapsed ? 'justify-content-center' : 'justify-content-end'}`}>
        <button
          className="btn btn-sm btn-outline-secondary border-0"
          onClick={onToggle}
          title={collapsed ? 'Expandir menu' : 'Recolher menu'}
        >
          <i className={`bi ${collapsed ? 'bi-chevron-double-right' : 'bi-chevron-double-left'}`}></i>
        </button>
      </div>

      <ul className="nav flex-column gap-1 px-2">
        {menuItems.map(({ to, label, icon, end }) => (
          <li key={to} className="nav-item">
            <NavLink
              to={to}
              end={end}
              onClick={handleNavClick}
              title={collapsed ? label : ''}
              className={({ isActive }) =>
                `nav-link d-flex align-items-center gap-2 rounded px-3 py-2 ${
                  isActive
                    ? 'active bg-primary text-white fw-semibold'
                    : 'text-dark'
                }`
              }
            >
              <i className={`bi ${icon} flex-shrink-0`}></i>
              <span className="nav-label">{label}</span>
            </NavLink>
          </li>
        ))}
      </ul>
    </aside>
  )
}
