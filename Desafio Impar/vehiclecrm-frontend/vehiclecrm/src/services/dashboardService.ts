import api from './api'

export interface DashboardCards {
  vehicles: number
  customers: number
  opportunities: number
  soldVehiclesTotalValue: number
}

export interface StatusCount {
  status: string
  count: number
}

export interface DashboardData {
  cards: DashboardCards
  vehicleStatus: StatusCount[]
  opportunityStatus: StatusCount[]
}

export async function getDashboard(): Promise<DashboardData> {
  const response = await api.get<DashboardData>('dashboard')
  return response.data
}
