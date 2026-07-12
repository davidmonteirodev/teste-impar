import api from './api'
import type { Customer, CreateCustomerDTO, UpdateCustomerDTO, CustomerFilters } from '../types'
import type { PaginatedResponse } from '../types'

export interface CustomerQueryParams extends CustomerFilters {
  page: number
  pageSize: number
}

export const customerService = {
  getCustomers: (params: CustomerQueryParams, signal?: AbortSignal) => {
    const query: Record<string, string | number> = {
      page: params.page,
      pagesize: params.pageSize,
    }
    if (params.name) query.name = params.name
    if (params.email) query.email = params.email
    if (params.phone) query.phone = params.phone
    if (params.mainInterest != null) query.maininterest = params.mainInterest
    return api.get<PaginatedResponse<Customer>>('customers', { params: query, signal })
  },
  getById: (id: number) => api.get<Customer>(`customers/${id}`),
  create: (data: CreateCustomerDTO) => api.post<Customer>('customers', data),
  update: (id: number, data: UpdateCustomerDTO) =>
    api.put<Customer>('customers', { id, ...data }),
  remove: (id: number) => api.delete(`customers/${id}`),
}
