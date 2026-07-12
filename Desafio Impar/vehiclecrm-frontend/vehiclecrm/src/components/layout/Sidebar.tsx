import { NavLink } from 'react-router-dom'

const menuItems = [
  { to: '/', label: 'Home', icon: 'bi-house-fill', end: true },
  { to: '/vehicles', label: 'Veículos', icon: 'bi-car-front-fill', end: false },
  { to: '/customers', label: 'Clientes', icon: 'bi-people-fill', end: false },
]

export default function Sidebar() {
  return (
    <aside
      className="bg-white border-end d-flex flex-column py-3"
      style={{ width: '220px', minWidth: '220px' }}
    >
      <ul className="nav flex-column gap-1 px-2">
        {menuItems.map(({ to, label, icon, end }) => (
          <li key={to} className="nav-item">
            <NavLink
              to={to}
              end={end}
              className={({ isActive }) =>
                `nav-link d-flex align-items-center gap-2 rounded px-3 py-2 ${
                  isActive
                    ? 'active bg-primary text-white fw-semibold'
                    : 'text-dark'
                }`
              }
            >
              <i className={`bi ${icon}`}></i>
              {label}
            </NavLink>
          </li>
        ))}
      </ul>
    </aside>
  )
}
