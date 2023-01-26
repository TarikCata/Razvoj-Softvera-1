import {
  Component,
  Input,
  OnInit,
  Output,
  EventEmitter,
  OnChanges,
} from '@angular/core';
import { MojConfig } from 'src/app/moj-config';
import { HttpClient } from '@angular/common/http';

declare function porukaSuccess(s: string): any;
declare function porukaError(a: string): any;

@Component({
  selector: 'app-student-edit',
  templateUrl: './student-edit.component.html',
  styleUrls: ['./student-edit.component.css'],
})
export class StudentEditComponent implements OnInit, OnChanges {
  @Input() dialogData: any;
  @Output() callback = new EventEmitter<boolean>();
  @Output() closeDialog = new EventEmitter<boolean>();
  opstinaData: any;
  opstina: any;

  constructor(private httpClient: HttpClient) {}

  ngOnChanges() {
    if (!!this.opstinaData) this.getDefaultOpstina();
  }

  ngOnInit() {
    this.loadOpstine();
  }

  loadOpstine = () => {
    const url = `${MojConfig.adresa_servera}/Opstina/GetByAll`;
    this.httpClient.get(url, MojConfig.http_opcije()).subscribe((res) => {
      if (!!res) {
        this.opstinaData = res;
        this.getDefaultOpstina();
      }
    });
  };

  getDefaultOpstina = () => {
    const { opstina_rodjenja } = this.dialogData.studentData;
    if (!opstina_rodjenja)
      this.opstina = this.opstinaData.find(({ id }: any) => id == 2).id;
    else this.opstina = opstina_rodjenja.id;
  };

  saveChanges = () => {
    const url = `${MojConfig.adresa_servera}/Student/Upsert`;
    const { studentData } = this.dialogData;
    const body = { ...studentData, opstina_rodjenja_id: this.opstina };
    this.httpClient
      .post(url, { ...body }, MojConfig.http_opcije())
      .subscribe((res) => {
        if (!!res) {
          porukaSuccess('Uspjesno sacuvane promjene!');
          this.callback.next(true);
          this.closeDialog.next(true);
        } else porukaError('Greska!');
      });
  };

  dismiss = () => this.closeDialog.next(true);
}
