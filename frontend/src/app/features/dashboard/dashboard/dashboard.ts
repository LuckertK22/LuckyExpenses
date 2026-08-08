import { Component, computed, inject, resource } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { firstValueFrom } from 'rxjs';
import {
  ChartComponent,
  ApexAxisChartSeries,
  ApexChart,
  ApexDataLabels,
  ApexLegend,
  ApexNonAxisChartSeries,
  ApexPlotOptions,
  ApexStroke,
  ApexTooltip,
  ApexXAxis,
} from 'ng-apexcharts';

import { DashboardService } from '../../../core/services/dashboard.service';
import { Spinner } from '../../../shared/components/spinner/spinner';
import { EmptyState } from '../../../shared/components/empty-state/empty-state';

@Component({
  selector: 'app-dashboard',
  imports: [ChartComponent, Spinner, EmptyState, CurrencyPipe],
  template: `
    <div class="dashboard">
      <header class="dashboard__header">
        <div>
          <h1 class="dashboard__title">Dashboard</h1>
          <p class="dashboard__subtitle">
            Resumen de {{ monthName() }} {{ year }}
          </p>
        </div>
      </header>

      @if (isLoading()) {
        <div class="dashboard__loading">
          <app-spinner size="lg" />
        </div>
      } @else if (hasError()) {
        <app-empty-state
          title="No se pudo cargar el resumen"
          description="Ocurrió un error al consultar el dashboard."
        />
      } @else if (hasValue()) {
        <section class="dashboard__kpis" aria-label="Indicadores del mes">
          <article class="kpi">
            <span class="kpi__label">Total gastado</span>
            <span class="kpi__value">{{ totalAmount() | currency }}</span>
            <span
              class="kpi__trend"
              [class.kpi__trend--up]="changePercent() > 0"
              [class.kpi__trend--down]="changePercent() <= 0"
            >
              {{ changePercentText() }} vs mes anterior
            </span>
          </article>
          <article class="kpi">
            <span class="kpi__label">Gastos</span>
            <span class="kpi__value">{{ totalExpenses() }}</span>
            <span class="kpi__hint">transacciones</span>
          </article>
          <article class="kpi">
            <span class="kpi__label">Promedio por gasto</span>
            <span class="kpi__value">{{ averageAmount() | currency }}</span>
            <span class="kpi__hint">por transacción</span>
          </article>
        </section>

        <section class="dashboard__charts">
          <article class="panel">
            <h2 class="panel__title">Mes actual vs anterior</h2>
            <apx-chart
              [chart]="comparisonChart()"
              [series]="comparisonSeries()"
              [xaxis]="comparisonXAxis()"
              [colors]="['var(--le-primary)', 'var(--le-text-subtle)']"
              [plotOptions]="comparisonPlotOptions"
              [dataLabels]="noDataLabels"
              [tooltip]="tooltipOptions"
              [legend]="hideLegend"
            />
          </article>
          <article class="panel">
            <h2 class="panel__title">Por categoría</h2>
            @if (hasCategoryData()) {
              <apx-chart
                [chart]="donutChart()"
                [series]="categorySeries()"
                [labels]="categoryLabels()"
                [colors]="categoryColors"
                [stroke]="donutStroke"
                [dataLabels]="noDataLabels"
                [tooltip]="tooltipOptions"
                [legend]="donutLegend"
                [plotOptions]="donutPlotOptions"
              />
            } @else {
              <app-empty-state
                title="Sin datos de categoría"
                description="Registra gastos para ver la distribución por categoría."
              />
            }
          </article>
        </section>
      }
    </div>
  `,
  styles: [
    `
      .dashboard {
        display: flex;
        flex-direction: column;
        gap: var(--le-space-6);
      }
      .dashboard__header {
        display: flex;
        align-items: center;
        justify-content: space-between;
        gap: var(--le-space-4);
      }
      .dashboard__title {
        font-size: var(--le-fs-2xl);
      }
      .dashboard__subtitle {
        font-size: var(--le-fs-sm);
        color: var(--le-text-muted);
        margin-top: var(--le-space-1);
      }
      .dashboard__loading {
        display: flex;
        justify-content: center;
        padding: var(--le-space-10);
        color: var(--le-primary);
      }

      .dashboard__kpis {
        display: grid;
        grid-template-columns: 1fr;
        gap: var(--le-space-4);
      }
      .kpi {
        display: flex;
        flex-direction: column;
        gap: var(--le-space-1);
        padding: var(--le-space-5);
        background: var(--le-surface);
        border: 1px solid var(--le-border);
        border-radius: var(--le-radius-md);
      }
      .kpi__label {
        font-size: var(--le-fs-sm);
        color: var(--le-text-muted);
      }
      .kpi__value {
        font-size: var(--le-fs-2xl);
        font-weight: var(--le-fw-bold);
        color: var(--le-text);
      }
      .kpi__trend {
        font-size: var(--le-fs-xs);
        font-weight: var(--le-fw-medium);
      }
      .kpi__trend--up {
        color: var(--le-danger);
      }
      .kpi__trend--down {
        color: var(--le-success);
      }
      .kpi__hint {
        font-size: var(--le-fs-xs);
        color: var(--le-text-subtle);
      }

      .dashboard__charts {
        display: grid;
        grid-template-columns: 1fr;
        gap: var(--le-space-4);
      }
      .panel {
        padding: var(--le-space-5);
        background: var(--le-surface);
        border: 1px solid var(--le-border);
        border-radius: var(--le-radius-md);
      }
      .panel__title {
        font-size: var(--le-fs-md);
        margin-bottom: var(--le-space-4);
      }

      @media (min-width: 769px) {
        .dashboard__kpis {
          grid-template-columns: repeat(3, 1fr);
        }
        .dashboard__charts {
          grid-template-columns: 1.2fr 1fr;
        }
      }
    `,
  ],
})
export class Dashboard {
  private readonly dashboardService = inject(DashboardService);

  protected readonly today = new Date();
  protected readonly year = this.today.getFullYear();
  protected readonly month = this.today.getMonth() + 1;

  protected readonly summaryResource = resource({
    loader: () =>
      firstValueFrom(
        this.dashboardService.getSummary({ year: this.year, month: this.month }),
      ),
  });

  protected readonly summary = this.summaryResource.value;
  protected readonly hasValue = () => this.summaryResource.hasValue();
  protected readonly isLoading = this.summaryResource.isLoading;
  protected readonly hasError = computed(() => !!this.summaryResource.error());

  protected readonly noDataLabels: ApexDataLabels = { enabled: false };
  protected readonly hideLegend: ApexLegend = { show: false };
  protected readonly donutLegend: ApexLegend = { position: 'bottom' };
  protected readonly donutStroke: ApexStroke = { colors: ['transparent'] };
  protected readonly donutPlotOptions: ApexPlotOptions = {
    pie: {
      donut: { size: '72%', labels: { show: false } },
    },
  };
  protected readonly tooltipOptions: ApexTooltip = {
    y: {
      formatter: (value: number) => this.formatMoney(value),
    },
  };
  protected readonly categoryColors = [
    '#059669',
    '#10b981',
    '#34d399',
    '#f59e0b',
    '#6366f1',
    '#ec4899',
    '#14b8a6',
    '#f97316',
    '#8b5cf6',
    '#84cc16',
  ];

  protected readonly monthName = computed(() =>
    new Intl.DateTimeFormat('es', { month: 'long' }).format(
      new Date(this.year, this.month - 1, 1),
    ),
  );

  protected readonly totalAmount = computed(() => this.summary()?.totalAmount ?? 0);
  protected readonly totalExpenses = computed(() => this.summary()?.totalExpenses ?? 0);
  protected readonly averageAmount = computed(() => this.summary()?.averageAmount ?? 0);
  protected readonly previousTotalAmount = computed(
    () => this.summary()?.previousTotalAmount ?? 0,
  );
  protected readonly changePercent = computed(() => this.summary()?.changePercent ?? 0);

  protected readonly changePercentText = computed(() => {
    const value = this.changePercent();
    const sign = value > 0 ? '+' : '';
    return `${sign}${value.toFixed(1)}%`;
  });

  protected readonly comparisonSeries = computed<ApexAxisChartSeries>(() => [
    {
      name: 'Gastado',
      data: [this.previousTotalAmount(), this.totalAmount()],
    },
  ]);

  protected readonly comparisonXAxis = computed<ApexXAxis>(() => ({
    categories: [this.previousMonthName(), this.monthName()],
  }));

  protected readonly comparisonChart = computed<ApexChart>(() => ({
    type: 'bar',
    height: 280,
    fontFamily: 'inherit',
    toolbar: { show: false },
  }));

  protected readonly comparisonPlotOptions: ApexPlotOptions = {
    bar: {
      borderRadius: 4,
      columnWidth: '45%',
      distributed: true,
    },
  };

  protected readonly categorySeries = computed<ApexNonAxisChartSeries>(() =>
    (this.summary()?.byCategory ?? []).map((item) => item.amount),
  );

  protected readonly categoryLabels = computed(() =>
    (this.summary()?.byCategory ?? []).map((item) => item.categoryName),
  );

  protected readonly hasCategoryData = computed(() =>
    (this.summary()?.byCategory ?? []).some((item) => item.amount > 0),
  );

  protected readonly donutChart = computed<ApexChart>(() => ({
    type: 'donut',
    height: 280,
    fontFamily: 'inherit',
    toolbar: { show: false },
  }));

  private previousMonthName(): string {
    const date = new Date(this.year, this.month - 2, 1);
    return new Intl.DateTimeFormat('es', { month: 'long' }).format(date);
  }

  private formatMoney(value: number): string {
    return new Intl.NumberFormat('es', {
      style: 'currency',
      currency: 'USD',
    }).format(value);
  }
}
