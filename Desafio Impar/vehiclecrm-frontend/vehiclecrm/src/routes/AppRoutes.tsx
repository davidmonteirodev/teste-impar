import { Routes, Route } from 'react-router-dom'
import MainLayout from '../components/layout/MainLayout'
import Home from '../pages/Home'
import Vehicles from '../pages/Vehicles'
import NewVehicle from '../pages/Vehicles/NewVehicle'

export default function AppRoutes() {
  return (
    <Routes>
      <Route path="/" element={<MainLayout />}>
        <Route index element={<Home />} />
        <Route path="vehicles" element={<Vehicles />} />
        <Route path="vehicles/new" element={<NewVehicle />} />
      </Route>
    </Routes>
  )
}
