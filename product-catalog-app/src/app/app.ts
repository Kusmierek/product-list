import { Component } from '@angular/core';
import { NgIcon } from '@ng-icons/core';
import { ProductListComponent } from './components/product-list/product-list.component';
import { ProductFormComponent } from './components/product-form/product-form.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [ProductListComponent, ProductFormComponent, NgIcon],
  templateUrl: './app.html',
  styleUrls: ['./app.css']
})
export class App {}
