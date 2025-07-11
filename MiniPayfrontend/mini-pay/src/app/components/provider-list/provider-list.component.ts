import { Component, OnInit } from '@angular/core';


import { CommonModule, DatePipe } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/api.service';
import { PaymentProvider } from '../../models/payment-provider.model';

@Component({
  selector: 'app-provider-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './provider-list.component.html',
  styleUrls: ['./provider-list.component.scss']
})
export class ProviderListComponent implements OnInit {
  providers: PaymentProvider[] = [];
  isLoading = true;

  constructor(private apiService: ApiService) { }

  ngOnInit(): void {
    this.loadProviders();
  }
  getProviderLink(name: string): string {
  switch (name.toLowerCase()) {
    case 'paypal':
      return 'https://www.paypal.com';
    case 'stripe':
      return 'https://stripe.com';
    case 'visa':
      return 'https://visa.com';
    default:
      return '#';
  }
}



  loadProviders(): void {
    this.isLoading = true;
    this.apiService.getProviders().subscribe({
      next: (providers) => {
        this.providers = providers;
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }

  toggleStatus(id: string): void {
    this.apiService.toggleProviderStatus(id).subscribe({
      next: () => {
        this.loadProviders();
      }
    });
  }

  deleteProvider(id: string): void {
    if (confirm('Are you sure you want to delete this provider?')) {
      this.apiService.deleteProvider(id).subscribe({
        next: () => {
          this.loadProviders();
        }
      });
    }
  }
}
