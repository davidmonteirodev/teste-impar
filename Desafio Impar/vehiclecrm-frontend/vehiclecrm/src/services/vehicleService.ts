import api from './api'
import type { Vehicle, CreateVehicleDTO, UpdateVehicleDTO, VehicleFilters } from '../types'
import type { PaginatedResponse } from '../types'

export interface VehicleQueryParams extends VehicleFilters {
  page: number
  pageSize: number
}

export const vehicleService = {
  getVehicles: (params: VehicleQueryParams, signal?: AbortSignal) => {
    const query: Record<string, string | number> = {
      page: params.page,
      pagesize: params.pageSize,
    }
    if (params.brand) query.brand = params.brand
    if (params.model) query.model = params.model
    if (params.yearFrom != null) query.yearfrom = params.yearFrom
    if (params.yearTo != null) query.yearto = params.yearTo
    if (params.priceFrom != null) query.pricefrom = params.priceFrom
    if (params.priceTo != null) query.priceto = params.priceTo
    if (params.color) query.color = params.color
    if (params.mileageFrom != null) query.mileagefrom = params.mileageFrom
    if (params.mileageTo != null) query.mileageto = params.mileageTo
    return api.get<PaginatedResponse<Vehicle>>('vehicles', { params: query, signal })
  },
  getById: (id: number) => api.get<Vehicle>(`vehicles/${id}`),
  create: (data: CreateVehicleDTO) => api.post<Vehicle>('vehicles', data),
  update: (id: number, data: UpdateVehicleDTO) =>
    api.put<Vehicle>('vehicles', { id, ...data }),
  remove: (id: number) => api.delete(`vehicles/${id}`),
}
