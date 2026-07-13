export type CustomerInterest = 1 | 2 | 3 | 4 | 5 | 6

export interface Customer {
  id: number
  name: string
  email: string
  phone: string
  mainInterest: CustomerInterest
  createdAt: string
  updatedAt: string
}

export interface CreateCustomerDTO {
  name: string
  email: string
  phone: string
  mainInterest: CustomerInterest
}

export type UpdateCustomerDTO = Partial<Omit<CreateCustomerDTO, 'email'>>

export interface CustomerFilters {
  name?: string
  email?: string
  phone?: string
  mainInterest?: number
}
