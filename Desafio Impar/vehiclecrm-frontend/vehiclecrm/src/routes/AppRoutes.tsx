import { Routes, Route } from 'react-router-dom'
import MainLayout from '../components/layout/MainLayout'
import Home from '../pages/Home'
import Vehicles from '../pages/Vehicles'
import NewVehicle from '../pages/Vehicles/NewVehicle'
import EditVehicle from '../pages/Vehicles/EditVehicle'
import Customers from '../pages/Customers'
import NewCustomer from '../pages/Customers/NewCustomer'
import EditCustomer from '../pages/Customers/EditCustomer'

export default function AppRoutes() {
  return (
    <Routes>
      <Route path="/" element={<MainLayout />}>
        <Route index element={<Home />} />
        <Route path="vehicles" element={<Vehicles />} />
        <Route path="vehicles/new" element={<NewVehicle />} />
        <Route path="vehicles/:id/edit" element={<EditVehicle />} />
        <Route path="customers" element={<Customers />} />
        <Route path="customers/new" element={<NewCustomer />} />
        <Route path="customers/:id/edit" element={<EditCustomer />} />
      </Route>
    </Routes>
  )
}
