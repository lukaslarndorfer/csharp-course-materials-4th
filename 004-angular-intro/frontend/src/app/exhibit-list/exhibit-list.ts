import {
  MatCell,
  MatCellDef, MatColumnDef, MatHeaderCell,
  MatHeaderCellDef,
  MatHeaderRow,
  MatHeaderRowDef,
  MatRow,
  MatRowDef,
  MatTable
} from '@angular/material/table';
import {MatProgressBar} from '@angular/material/progress-bar';
import {MatTooltip} from '@angular/material/tooltip';
import {Router} from '@angular/router';
import {Component, computed, inject, OnInit, signal, Signal, WritableSignal} from '@angular/core';
import {ExhibitInfo, ExhibitsService} from '../../core/services/exhibit-service';
import {ExhibitListTable} from './exhibit-list-table/exhibit-list-table';

@Component({
  selector: "app-exhibit-list",
  imports: [
    MatTable,
    MatHeaderCell,
    MatColumnDef,
    MatCell,
    MatCellDef,
    MatHeaderCellDef,
    MatHeaderRow,
    MatRow,
    MatRowDef,
    MatHeaderRowDef,
    MatProgressBar,
    MatTooltip,
    ExhibitListTable
  ],
  templateUrl: "./exhibit-list.html",
  styleUrl: "./exhibit-list.scss"
})
class ExhibitList implements OnInit {
  protected readonly displayedColumns: string[] = ["name", "serviceStartYear", "serviceEndYear"];
  protected exhibits: WritableSignal<ExhibitInfo[]> = signal([]);
  protected loading: Signal<boolean> = computed(() => this.exhibits().length === 0);
  private readonly service: ExhibitsService = inject(ExhibitsService);
  private readonly router: Router = inject(Router);

  public async ngOnInit(): Promise<void> {
    const data: ExhibitInfo[] | undefined = await this.service.getExhibits();
    if (data) {
      this.exhibits.set(data);
    } else {
      console.log("Error getting exhibits");
    }
  }

  public async handleRowClicked(exhibit: ExhibitInfo): Promise<void> {
    const detailsUrl = `/exhibits/${exhibit.id}`;
    await this.router.navigate([detailsUrl]);
  }
}

export default ExhibitList
