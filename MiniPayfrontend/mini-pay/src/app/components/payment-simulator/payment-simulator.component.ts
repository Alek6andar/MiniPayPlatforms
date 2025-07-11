import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../services/api.service';
import { PaymentRequestPayload } from '../../models/payment-request.model';

import { PaymentProvider } from '../../models/payment-provider.model';

@Component({
  selector: 'app-payment-simulator',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './payment-simulator.component.html',
  styleUrls: ['./payment-simulator.component.scss']
})
export class PaymentSimulatorComponent implements OnInit {
  form: FormGroup;
  providers: PaymentProvider[] = [];
  isLoading = false;
  activeProviders: PaymentProvider[] = [];
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private apiService: ApiService,
    private router: Router
  ) {
    this.form = this.fb.group({
      providerId: ['', Validators.required],
      amount: ['', [Validators.required, Validators.min(0.01)]],
      currency: ['', Validators.required],
      description: ['']
    });
  }

  ngOnInit(): void {
    this.loadProviders();
  }

  loadProviders(): void {
    this.isLoading = true;
    this.apiService.getProviders().subscribe({
      next: (providers) => {
        this.providers = providers;
        this.activeProviders = providers.filter(p => p.isActive);
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }

  onProviderChange(): void {
    const providerId = this.form.get('providerId')?.value;
    const provider = this.providers.find(p => p.id === providerId);
    if (provider) {
      this.form.get('currency')?.setValue(provider.supportedCurrency);
    }
  }

  onSubmit(): void {
    if (this.form.invalid) return;

    const request: PaymentRequestPayload = {
      providerId: this.form.value.providerId,
      amount: this.form.value.amount,
      currency: this.form.value.currency,
      description: this.form.value.description,
      referenceId: `ORDER-${Math.random().toString(36).substring(2, 10)}`
    };

    this.isLoading = true;
    this.errorMessage = '';

    this.apiService.processPayment(request).subscribe({
      next: (response) => {
        this.router.navigate(['/transaction'], {
          state: { transaction: response }
        });
      },
      error: (error) => {
        this.errorMessage = error.message || 'Payment processing failed';
        this.isLoading = false;
      }
    });
  }
}
