import { Component, output, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { ProductService } from '../../services/product.service';
import { CreateProductDto } from '../../models/product.model';

@Component({
  selector: 'app-product-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './product-form.component.html',
  styleUrl: './product-form.component.css'
})
export class ProductFormComponent {
  readonly productAdded = output<void>();
  private productService = inject(ProductService);

  model: CreateProductDto = { code: '', name: '', price: 0 };
  submitting = signal(false);
  successMessage = signal('');
  errorMessage = signal('');

  onSubmit(form: NgForm): void {
    if (!this.model.code || !this.model.name || this.model.price <= 0) return;

    this.submitting.set(true);
    this.successMessage.set('');
    this.errorMessage.set('');

    this.productService.create(this.model).subscribe({
      next: () => {
        this.successMessage.set(`Product "${this.model.name}" added successfully!`);
        form.resetForm({ code: '', name: '', price: 0 });
        this.submitting.set(false);
        this.productAdded.emit();
      },
      error: () => {
        this.errorMessage.set('Failed to add product. Is the API running?');
        this.submitting.set(false);
      }
    });
  }
}
