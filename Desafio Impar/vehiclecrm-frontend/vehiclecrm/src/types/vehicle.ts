export interface Vehicle {
  id: number
  brand: string
  model: string
  year: number
  color: string
  plate: string
  createdAt: string
  updatedAt: string
}

export interface CreateVehicleDTO {
  brand: string
  model: string
  year: number
  color: string
  plate: string
}

export type UpdateVehicleDTO = Partial<CreateVehicleDTO>
