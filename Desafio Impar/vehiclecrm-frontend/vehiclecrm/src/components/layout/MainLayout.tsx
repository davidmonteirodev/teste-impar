import { useState, useEffect } from 'react'
import { Outlet } from 'react-router-dom'
import Navbar from './Navbar'
import Sidebar from './Sidebar'

export default function MainLayout() {
  // Mobile começa recolhido; desktop começa expandido
  const [collapsed, setCollapsed] = useState(() => window.innerWidth < 768)

  const toggle = () => setCollapsed(c => !c)

  // Fecha automaticamente ao entrar em viewport mobile
  useEffect(() => {
    const mq = window.matchMedia('(max-width: 767.98px)')
    const handleChange = (e: MediaQueryListEvent) => {
      if (e.matches) setCollapsed(true)
    }
    mq.addEventListener('change', handleChange)
    return () => mq.removeEventListener('change', handleChange)
  }, [])

  return (
    <div className="d-flex flex-column vh-100">
      <Navbar onToggleSidebar={toggle} />
      <div className="d-flex flex-grow-1 overflow-hidden position-relative">
        {/* Overlay escuro no mobile quando o sidebar está aberto */}
        {!collapsed && (
          <div
            className="sidebar-overlay d-md-none"
            onClick={() => setCollapsed(true)}
          />
        )}
        <Sidebar collapsed={collapsed} onToggle={toggle} />
        <main className="flex-grow-1 p-4 overflow-auto bg-light">
          <Outlet />
        </main>
      </div>
    </div>
  )
}
