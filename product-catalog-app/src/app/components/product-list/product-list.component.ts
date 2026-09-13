import { Component, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgIcon } from '@ng-icons/core';
import { ProductService } from '../../services/product.service';
import { AlertState, Product } from '../../models/product.model';
import { ProductRowComponent } from '../product-row/product-row.component';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, ProductRowComponent, NgIcon],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css',
})
export class ProductListComponent {
  private productService = inject(ProductService);

  state = signal<{ products: Product[]; loading: boolean; alert: AlertState }>({
    products: [],
    loading: false,
    alert: null,
  });

  constructor() {
    this.loadProducts();
  }

  loadProducts(): void {
    this.state.update(s => ({ ...s, loading: true, alert: null }));
    this.productService.getAll().subscribe({
      next: (products) => this.state.update(s => ({ ...s, products, loading: false })),
      error: () => this.state.update(s => ({
        ...s,
        loading: false,
        alert: { type: 'error', message: 'Failed to load products. Is the API running?' },
      })),
    });
  }
}
