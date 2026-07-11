import api from './api'
import type { Vehicle, CreateVehicleDTO, UpdateVehicleDTO } from '../types'

// CRUD operations serão implementadas aqui
export const vehicleService = {
  getAll: () => api.get<Vehicle[]>('/vehicles'),
  getById: (id: number) => api.get<Vehicle>(`/vehicles/${id}`),
  create: (data: CreateVehicleDTO) => api.post<Vehicle>('/vehicles', data),
  update: (id: number, data: UpdateVehicleDTO) =>
    api.put<Vehicle>(`/vehicles/${id}`, data),
  remove: (id: number) => api.delete(`/vehicles/${id}`),
}
