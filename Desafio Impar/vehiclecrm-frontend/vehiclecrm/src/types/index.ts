export type { Vehicle, CreateVehicleDTO, UpdateVehicleDTO, VehicleStatus, VehicleFilters } from './vehicle'
export type { Customer, CreateCustomerDTO, UpdateCustomerDTO, CustomerInterest, CustomerFilters } from './customer'

export interface PaginatedResponse<T> {
  items: T[]
  totalItems: number
  page: number
  pageSize: number
  totalPages: number
  hasPreviousPage: boolean
  hasNextPage: boolean
}

export interface PaginationParams {
  page: number
  pageSize: number
}
