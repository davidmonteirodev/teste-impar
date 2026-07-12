import api from './api'
import type { Customer, CreateCustomerDTO, UpdateCustomerDTO } from '../types'

export const customerService = {
  getAll: () => api.get<Customer[]>('customers'),
  getById: (id: number) => api.get<Customer>(`customers/${id}`),
  create: (data: CreateCustomerDTO) => api.post<Customer>('customers', data),
  update: (id: number, data: UpdateCustomerDTO) =>
    api.put<Customer>(`customers/${id}`, data),
  remove: (id: number) => api.delete(`customers/${id}`),
}
