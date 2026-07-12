export type SaleOpportunityStatus = 1 | 2 | 3 | 4 | 5

export interface SaleOpportunity {
  id: number
  proposedValue: number
  status: SaleOpportunityStatus
  notes?: string
  createDate: string
  modificationDate: string | null
  vehicle: {
    vehicleId: number
    model: string
  }
  customer: {
    customerId: number
    name: string
  }
}

export interface CreateSaleOpportunityDTO {
  customerId: number
  vehicleId: number
  status: SaleOpportunityStatus
  proposedValue: number
  notes?: string
}

export interface UpdateSaleOpportunityDTO {
  id: number
  customerId: number
  vehicleId: number
  status: SaleOpportunityStatus
  proposedValue: number
  notes?: string
}

export interface SaleOpportunityFilters {
  vehicleModel?: string
  customerName?: string
  proposedValueFrom?: number
  proposedValueTo?: number
  status?: SaleOpportunityStatus
}
