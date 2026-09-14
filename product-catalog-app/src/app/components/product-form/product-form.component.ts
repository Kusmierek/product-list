import { Component, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { NgIcon } from '@ng-icons/core';
import { ProductService } from '../../services/product.service';
import { ProductRefreshService } from '../../services/product-refresh.service';
import { AlertState, CreateProductDto } from '../../models/product.model';

@Component({
  selector: 'app-product-form',
  imports: [CommonModule, FormsModule, NgIcon],
  templateUrl: './product-form.component.html',
  styleUrl: './product-form.component.css',
})
export class ProductFormComponent {
  private readonly productService = inject(ProductService);
  private readonly refreshService = inject(ProductRefreshService);

  model: CreateProductDto = { code: '', name: '', price: 0 };
  state = signal<{ submitting: boolean; alert: AlertState }>({
    submitting: false,
    alert: null,
  });

  private parseError(err: HttpErrorResponse): string {
    if (err.status === 400 && err.error?.errors) {
      return Object.values(err.error.errors as Record<string, string[]>).flat().join(' ');
    }
    if (err.status === 409 && err.error?.error) {
      return err.error.error;
    }
    return 'Failed to add product. Please try again.';
  }

  onSubmit(form: NgForm): void {
    if (form.invalid || this.state().submitting) return;

    this.state.update(s => ({ ...s, submitting: true, alert: null }));

    this.productService.create(this.model).subscribe({
      next: () => {
        this.state.update(s => ({
          ...s,
          submitting: false,
          alert: { type: 'success', message: `Product "${this.model.name.length > 50 ? this.model.name.slice(0, 50) + '…' : this.model.name}" added successfully!` },
        }));
        form.resetForm({ code: '', name: '', price: 0 });
        this.refreshService.trigger();
      },
      error: (err: HttpErrorResponse) => {
        const message = this.parseError(err);
        this.state.update(s => ({ ...s, submitting: false, alert: { type: 'error', message } }));
      },
    });
  }
}
