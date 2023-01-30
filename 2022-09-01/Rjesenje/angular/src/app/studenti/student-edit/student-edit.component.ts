import { HttpClient } from '@angular/common/http';
import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
} from '@angular/core';
import { MojConfig } from '../../moj-config';

declare function porukaSuccess(s: string): any;
declare function porukaError(s: string): any;

@Component({
  selector: 'app-student-edit',
  templateUrl: './student-edit.component.html',
  styleUrls: ['./student-edit.component.css'],
})
export class StudentEditComponent implements OnInit, OnChanges {
  @Input() dialogData: any;
  @Output() closeDialog = new EventEmitter<boolean>();
  @Output() callback = new EventEmitter<boolean>();
  sveOpstine: any;
  opstina: any;

  constructor(private http: HttpClient) {}

  ngOnChanges() {
    if (!!this.sveOpstine) this.getDefaultOpstina();
  }

  ngOnInit() {
    this.loadOpstine();
  }

  loadOpstine = () => {
    const url = `${MojConfig.adresa_servera}/Opstina/GetByAll`;
    this.http.get(url, MojConfig.http_opcije()).subscribe((res) => {
      if (!!res) {
        this.sveOpstine = res;
        this.getDefaultOpstina();
      }
    });
  };

  getDefaultOpstina = () => {
    const { opstina_rodjenja } = this.dialogData.studentData;
    !!opstina_rodjenja
      ? (this.opstina = opstina_rodjenja.id)
      : (this.opstina = this.sveOpstine.find(({ id }: any) => id == 2).id);
  };

  saveChanges = () => {
    const { studentData } = this.dialogData;
    const body = { ...studentData, opstina_rodjenja_id: this.opstina };
    const url = `${MojConfig.adresa_servera}/Student/Upsert`;
    const res = this.http
      .post(url, { ...body }, MojConfig.http_opcije())
      .toPromise();
    res
      .then((data) => {
        if (!!data) {
          studentData.id == 0
            ? porukaSuccess(`Uspjesno dodan ${studentData.ime}!`)
            : porukaSuccess(
                `Uspjesno sacuvane promjene za ${studentData.ime}!`
              );
        }
      })
      .catch((err) => porukaError(err.error))
      .finally(() => {
        this.callback.next(true);
        this.close();
      });
  };

  close = () => this.closeDialog.next(true);
}
