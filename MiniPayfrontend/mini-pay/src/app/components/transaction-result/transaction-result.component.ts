import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { PaymentResponse } from '../../models/payment-response.model';

@Component({
  selector: 'app-transaction-result',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './transaction-result.component.html',
  styleUrls: ['./transaction-result.component.scss']
})
export class TransactionResultComponent implements OnInit {
  transaction: PaymentResponse | null = null;

  constructor(private router: Router) {}

  ngOnInit(): void {
    this.transaction = history.state['transaction'];

    if (!this.transaction) {
      this.router.navigate(['/simulate']);
    }
  }
}

