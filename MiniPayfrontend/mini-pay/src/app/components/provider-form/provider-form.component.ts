import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';

import { CommonModule } from '@angular/common';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-provider-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './provider-form.component.html',
  styleUrls: ['./provider-form.component.scss']
})
export class ProviderFormComponent implements OnInit {
  form: FormGroup;
  isEditMode = false;
  providerId: string | null = null;
  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private apiService: ApiService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.form = this.fb.group({
      name: ['', Validators.required],
      apiUrl: ['', [Validators.required, Validators.pattern('https?://.+')]],
      isActive: [true],
      supportedCurrency: ['', Validators.required],
      description: ['']
    });
  }

  ngOnInit(): void {
    this.providerId = this.route.snapshot.paramMap.get('id');
    this.isEditMode = !!this.providerId;

    if (this.isEditMode) {
      this.loadProvider();
    }
  }

  loadProvider(): void {
    this.isLoading = true;
    this.apiService.getProvider(this.providerId!).subscribe({
      next: (provider) => {
        this.form.patchValue(provider);
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.form.invalid) return;

    const providerData = this.form.value;
    this.isLoading = true;

    if (this.isEditMode) {
      this.apiService.updateProvider({ ...providerData, id: this.providerId }).subscribe({
        next: () => {
          this.router.navigate(['/providers']);
        },
        error: () => {
          this.isLoading = false;
        }
      });
    } else {
      this.apiService.createProvider(providerData).subscribe({
        next: () => {
          this.router.navigate(['/providers']);
        },
        error: () => {
          this.isLoading = false;
        }
      });
    }
  }
}
