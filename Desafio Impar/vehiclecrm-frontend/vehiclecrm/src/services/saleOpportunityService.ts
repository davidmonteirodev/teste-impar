import api from './api'
import type { SaleOpportunity, SaleOpportunityFilters, CreateSaleOpportunityDTO, UpdateSaleOpportunityDTO } from '../types'
import type { PaginatedResponse } from '../types'

export interface SaleOpportunityQueryParams extends SaleOpportunityFilters {
  page: number
  pageSize: number
}

export const saleOpportunityService = {
  getSaleOpportunities: (params: SaleOpportunityQueryParams, signal?: AbortSignal) => {
    const query: Record<string, string | number> = {
      Page: params.page,
      Pagesize: params.pageSize,
    }
    if (params.customerName) query.CustomerName = params.customerName
    if (params.vehicleModel) query.VehicleModel = params.vehicleModel
    if (params.status != null) query.Status = params.status
    if (params.proposedValueFrom != null) query.ProposedValueFrom = params.proposedValueFrom
    if (params.proposedValueTo != null) query.ProposedValueTo = params.proposedValueTo
    return api.get<PaginatedResponse<SaleOpportunity>>('sale-opportunities', { params: query, signal })
  },
  getById: (id: number) => api.get<SaleOpportunity>(`sale-opportunities/${id}`),
  create: (data: CreateSaleOpportunityDTO) => api.post<SaleOpportunity>('sale-opportunities', data),
  update: (data: UpdateSaleOpportunityDTO) => api.put<SaleOpportunity>('sale-opportunities', data),
  remove: (id: number) => api.delete(`sale-opportunities/${id}`),
}
