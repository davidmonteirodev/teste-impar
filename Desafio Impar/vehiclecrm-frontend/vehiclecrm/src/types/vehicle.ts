export type VehicleStatus = 1 | 2 | 3

export interface Vehicle {
  id: number
  brand: string
  model: string
  year: number
  price: number
  color: string
  mileage: number
  status: VehicleStatus
  plate?: string
  createdAt: string
  updatedAt: string
}

export interface CreateVehicleDTO {
  brand: string
  model: string
  year: number
  price: number
  color: string
  mileage: number
  status: VehicleStatus
  plate?: string
}

export type UpdateVehicleDTO = Partial<CreateVehicleDTO>

export interface VehicleFilters {
  brand?: string
  model?: string
  yearFrom?: number
  yearTo?: number
  priceFrom?: number
  priceTo?: number
  color?: string
  mileageFrom?: number
  mileageTo?: number
}
