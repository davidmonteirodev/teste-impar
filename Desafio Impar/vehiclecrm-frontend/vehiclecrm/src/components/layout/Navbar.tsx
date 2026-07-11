import { Link } from 'react-router-dom'

export default function Navbar() {
  return (
    <nav className="navbar navbar-dark bg-dark px-3 flex-shrink-0">
      <Link className="navbar-brand d-flex align-items-center gap-2" to="/">
        <i className="bi bi-car-front-fill fs-5"></i>
        VehicleCRM
      </Link>
    </nav>
  )
}
