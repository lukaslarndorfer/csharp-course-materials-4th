import {Component, computed, input, InputSignal, output, OutputEmitterRef, Signal, viewChild} from '@angular/core';
import {ExhibitInfo} from '../../../core/services/exhibit-service';
import {
  MatCell,
  MatCellDef,
  MatColumnDef,
  MatHeaderCell,
  MatHeaderCellDef,
  MatHeaderRow, MatHeaderRowDef, MatRow, MatRowDef,
  MatTable, MatTableDataSource
} from '@angular/material/table';
import {MatTooltip} from '@angular/material/tooltip';
import {MatSort, MatSortHeader} from '@angular/material/sort';

@Component({
  selector: 'app-exhibit-list-table',
  imports: [
    MatTable,
    MatColumnDef,
    MatHeaderCell,
    MatCell,
    MatCellDef,
    MatHeaderCellDef,
    MatHeaderRow,
    MatHeaderRowDef,
    MatRowDef,
    MatRow,
    MatTooltip,
    MatSort,
    MatSortHeader
  ],
  templateUrl: './exhibit-list-table.html',
  styleUrl: './exhibit-list-table.scss',
})
export class ExhibitListTable {
  public exhibits: InputSignal<ExhibitInfo[]> = input.required();
  public onExhibitSelected: OutputEmitterRef<ExhibitInfo> = output();
  private readonly sort: Signal<MatSort> = viewChild.required(MatSort);
  // Mit viewChild kann man direkt
  // auf das Element zugreifen 'getElementById'
  protected readonly displayedColumns: string[] = ["name", "serviceStartYear", "serviceEndYear"];
  protected readonly exhibitData : Signal<MatTableDataSource<ExhibitInfo>>
  = computed(() => {
    const src = new MatTableDataSource(this.exhibits());
    src.sort = this.sort();
    return src;
  });

  public handleRowClicked(exhibit: ExhibitInfo): void {
    this.onExhibitSelected.emit(exhibit);
  }
}
