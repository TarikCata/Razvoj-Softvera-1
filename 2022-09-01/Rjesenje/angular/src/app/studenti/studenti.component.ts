import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { MojConfig } from '../moj-config';
import { Router } from '@angular/router';
declare function porukaSuccess(a: string): any;
declare function porukaError(a: string): any;

@Component({
  selector: 'app-studenti',
  templateUrl: './studenti.component.html',
  styleUrls: ['./studenti.component.css'],
})
export class StudentiComponent implements OnInit {
  title: string = 'angularFIT2';
  filter_imeprezime: string = '';
  filter_opstina: string = '';
  studentPodaci: any;
  checked_opstina: boolean;
  checked_imeprezime: boolean;
  isModalOpen: boolean = false;
  dialogData: any;

  constructor(private httpKlijent: HttpClient, private router: Router) {}

  ngOnInit() {
    this.testirajWebApi();
  }

  testirajWebApi(params?: HttpParams) {
    const url = `${MojConfig.adresa_servera}/Student/GetAll`;
    this.httpKlijent.get(url, { params }).subscribe((res: any) => {
      if (!!res) this.studentPodaci = res;
    });
  }

  filter = () => {
    const params = new HttpParams()
      .append('ime_prezime', this.filter_imeprezime)
      .append('opstina', this.filter_opstina);
    this.testirajWebApi(params);
  };

  openCloseDialog = (student?: any) => {
    this.isModalOpen = true;
    if (!!student) {
      const { id, ime, prezime, broj_indeksa, opstina_rodjenja } = student;
      this.dialogData = {
        title: 'Edit Student',
        studentData: { id, ime, prezime, broj_indeksa, opstina_rodjenja },
      };
      return;
    }
    const ime = this.filter_imeprezime;
    this.dialogData = {
      title: 'Novi Student',
      studentData: {
        id: 0,
        ime: !!ime ? ime : '',
        prezime: '',
        broj_indeksa: '',
        opstina_rodjenja: null,
      },
    };
  };

  deleteStudent = (student: any) => {
    const { id, ime } = student;
    const url = `${MojConfig.adresa_servera}/Student/Delete/${id}`;
    const res = this.httpKlijent
      .delete(url, MojConfig.http_opcije())
      .toPromise();
    res
      .then((status) => {
        if (!!status) porukaSuccess(`Uspjesno izbrisan ${ime}`);
      })
      .catch((err) => {
        porukaError(err.error);
      })
      .finally(() => this.testirajWebApi());
  };

  closeDialog = () => (this.isModalOpen = false);

  redirect = (id: number) =>
    this.router.navigate(['student-maticnaknjiga', id]);
}
