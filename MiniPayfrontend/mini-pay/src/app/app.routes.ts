import { Routes } from '@angular/router';
import { ProviderListComponent } from './components/provider-list/provider-list.component';
import { ProviderFormComponent } from './components/provider-form/provider-form.component';
import { PaymentSimulatorComponent } from './components/payment-simulator/payment-simulator.component';
import { TransactionResultComponent } from './components/transaction-result/transaction-result.component';


export const routes: Routes = [
  { path: '', redirectTo: '/providers', pathMatch: 'full' },
  { path: 'providers', component: ProviderListComponent },
  { path: 'providers/new', component: ProviderFormComponent },
  { path: 'providers/:id/edit', component: ProviderFormComponent },
  { path: 'simulate', component: PaymentSimulatorComponent },
  { path: 'transaction', component: TransactionResultComponent },
  { path: '**', redirectTo: '/providers' }
];
