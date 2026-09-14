import { Component, signal, inject, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { NgIcon } from '@ng-icons/core';
import { ProductService } from '../../services/product.service';
import { ProductRefreshService } from '../../services/product-refresh.service';
import { AlertState, Product, ProductQuery, ProductSortBy, SortDirection } from '../../models/product.model';
import { ProductRowComponent } from '../product-row/product-row.component';

const ICON_SORT_ASC = 'heroChevronUp' as const;
const ICON_SORT_DESC = 'heroChevronDown' as const;
const ICON_SORT_NONE = 'heroChevronUpDown' as const;

const DEFAULT_QUERY: ProductQuery = {
  page: 1,
  pageSize: 10,
  sortBy: 'CreatedAt',
  sortDir: 'Desc',
};

const VALID_SORT_BY: ProductSortBy[] = ['CreatedAt', 'Name', 'Price'];
const VALID_SORT_DIR: SortDirection[] = ['Asc', 'Desc'];

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, ProductRowComponent, NgIcon],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css',
})
export class ProductListComponent {
  private readonly productService = inject(ProductService);
  private readonly refreshService = inject(ProductRefreshService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly searchSubject = new Subject<string>();

  state = signal<{ products: Product[]; total: number; loading: boolean; alert: AlertState; query: ProductQuery }>({
    products: [],
    total: 0,
    loading: false,
    alert: null,
    query: DEFAULT_QUERY,
  });

  totalPages = computed(() => Math.ceil(this.state().total / this.state().query.pageSize) || 1);

  paginationRange = computed(() => {
    const { page, pageSize } = this.state().query;
    const total = this.state().total;
    const from = (page - 1) * pageSize + 1;
    const to = Math.min(page * pageSize, total);
    return { from, to, total };
  });

  searchInput = computed(() => this.state().query.search ?? '');

  sortIcons = computed(() => {
    const { sortBy, sortDir } = this.state().query;
    const active = sortDir === 'Asc' ? ICON_SORT_ASC : ICON_SORT_DESC;
    return {
      Name: sortBy === 'Name' ? active : ICON_SORT_NONE,
      Price: sortBy === 'Price' ? active : ICON_SORT_NONE,
      CreatedAt: sortBy === 'CreatedAt' ? active : ICON_SORT_NONE,
    };
  });

  constructor() {
    this.searchSubject.pipe(
      debounceTime(400),
      distinctUntilChanged(),
      takeUntilDestroyed(),
    ).subscribe(search => this.navigate({ search: search || undefined, page: 1 }));

    this.route.queryParams.pipe(takeUntilDestroyed()).subscribe(params => {
      const query: ProductQuery = {
        page: Number(params['page']) || 1,
        pageSize: Number(params['pageSize']) || 10,
        sortBy: VALID_SORT_BY.includes(params['sortBy']) ? params['sortBy'] : 'CreatedAt',
        sortDir: VALID_SORT_DIR.includes(params['sortDir']) ? params['sortDir'] : 'Desc',
        search: params['search'] || undefined,
      };
      this.state.update(s => ({ ...s, query }));
      this.load(query);
    });

    this.refreshService.refresh$.pipe(takeUntilDestroyed())
      .subscribe(() => this.load(this.state().query));
  }

  loadProducts(): void {
    this.load(this.state().query);
  }

  private load(query: ProductQuery): void {
    this.state.update(s => ({ ...s, loading: true, alert: null }));
    this.productService.getAll(query).subscribe({
      next: ({ items, total }) =>
        this.state.update(s => ({ ...s, products: items, total, loading: false })),
      error: () => this.state.update(s => ({
        ...s,
        loading: false,
        alert: { type: 'error', message: 'Failed to load products. Is the API running?' },
      })),
    });
  }

  onSearchInput(event: Event): void {
    this.searchSubject.next((event.target as HTMLInputElement).value);
  }

  onSort(sortBy: ProductSortBy): void {
    const { sortBy: current, sortDir } = this.state().query;
    const newDir: SortDirection = current === sortBy && sortDir === 'Asc' ? 'Desc' : 'Asc';
    this.navigate({ sortBy, sortDir: newDir, page: 1 });
  }

  onPage(page: number): void {
    this.navigate({ page });
  }

  private navigate(partial: Partial<ProductQuery>): void {
    const next = { ...this.state().query, ...partial };
    this.router.navigate([], {
      queryParams: {
        page: next.page !== 1 ? next.page : null,
        pageSize: next.pageSize !== 10 ? next.pageSize : null,
        sortBy: next.sortBy !== 'CreatedAt' ? next.sortBy : null,
        sortDir: next.sortDir !== 'Desc' ? next.sortDir : null,
        search: next.search || null,
      },
    });
  }
}
