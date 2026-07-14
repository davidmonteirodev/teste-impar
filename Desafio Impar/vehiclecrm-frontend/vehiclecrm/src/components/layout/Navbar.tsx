import { Link } from 'react-router-dom'

interface NavbarProps {
  onToggleSidebar: () => void
}

export default function Navbar({ onToggleSidebar }: NavbarProps) {
  return (
    <nav className="navbar navbar-dark bg-dark px-3 flex-shrink-0">
      {/* Botão hamburguer visível apenas no mobile */}
      <button
        className="btn btn-dark d-md-none me-2 p-1"
        onClick={onToggleSidebar}
        aria-label="Abrir menu"
      >
        <i className="bi bi-list fs-4"></i>
      </button>
      <Link className="navbar-brand d-flex align-items-center gap-2" to="/">
        <i className="bi bi-car-front-fill fs-5"></i>
        VehicleCRM
      </Link>
    </nav>
  )
}
