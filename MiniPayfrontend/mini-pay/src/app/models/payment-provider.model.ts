export interface PaymentProvider {
  id: string;
  name: string;
  apiUrl: string;
  isActive: boolean;
  supportedCurrency: string;
  description?: string;
  createdAt: string;
  updatedAt?: string;
}
