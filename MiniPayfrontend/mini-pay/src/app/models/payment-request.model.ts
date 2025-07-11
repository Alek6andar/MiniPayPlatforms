export interface PaymentRequestPayload {
  providerId: string;
  amount: number;
  currency: string;
  description?: string;
  referenceId: string;
}
