import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Product } from '../../models/product.model';

@Component({
  selector: '[app-product-row]',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './product-row.component.html'
})
export class ProductRowComponent {
  product = input.required<Product>();
  index = input.required<number>();
}
