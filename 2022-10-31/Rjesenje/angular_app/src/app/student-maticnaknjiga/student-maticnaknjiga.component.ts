import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MojConfig } from '../moj-config';
import { HttpClient } from '@angular/common/http';
import { FormBuilder, Validators } from '@angular/forms';

declare function porukaSuccess(a: string): any;
declare function porukaError(a: string): any;

@Component({
  selector: 'app-student-maticnaknjiga',
  templateUrl: './student-maticnaknjiga.component.html',
  styleUrls: ['./student-maticnaknjiga.component.css'],
})
export class StudentMaticnaknjigaComponent implements OnInit {
  upisaneGodine: any;
  currentStudent: any;
  isOpen: boolean = false;
  currentDate = new Date();
  akademskeGodine: any;
  akademska: any;
  isObnova = false;
  godinaStudija: number;
  cijena: number;
  datum: any;
  constructor(
    private httpKlijent: HttpClient,
    private route: ActivatedRoute,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    let id;
    this.route.paramMap.subscribe((paramMap) => {
      id = paramMap.get('id');
    });
    if (!!id && typeof id != 'number') this.getStudent(id);
    this.getGodine();
    this.getAkademskeGodine();
  }

  getAkademskeGodine = () => {
    const url = `${MojConfig.adresa_servera}/AkademskeGodine/GetAll_ForCmb`;
    this.httpKlijent.get(url, MojConfig.http_opcije()).subscribe((res) => {
      if (!!res) this.akademskeGodine = res;
      else porukaError('Greska kod ucitavanja akademskih godina!');
    });
  };

  getStudent = (id: number) => {
    const url = `${MojConfig.adresa_servera}/Student/GetById?id=${id}`;
    this.httpKlijent.get(url, MojConfig.http_opcije()).subscribe((res: any) => {
      if (!!res) {
        const { id, ime, prezime, ...rest } = res;
        this.currentStudent = { id, ime, prezime };
      } else porukaError('Greska kod ucitavanja studenta!');
    });
  };

  getGodine = () => {
    const url = `${MojConfig.adresa_servera}/UpisaneGodine/GetAll`;
    this.httpKlijent.get(url, MojConfig.http_opcije()).subscribe((res: any) => {
      if (!!res) this.upisaneGodine = res;
      else porukaError('Greska kod ucitavanja studenta!');
    });
  };

  saveGodinu = () => {
    const url = `${MojConfig.adresa_servera}/UpisaneGodine/Add`;
    const obj = {
      godinaStudija: this.godinaStudija,
      akademskaGodinaId: this.akademska,
      cijenaSkolarine: this.cijena,
      datumOvjere: this.currentDate,
      obnova: this.isObnova,
    };
    this.httpKlijent
      .post(url, { ...obj }, MojConfig.http_opcije())
      .subscribe((res) => {
        if (!!res) {
          this.getGodine();
          this.isOpen = false;
        } else porukaError('Greska kod dodavanja godine!');
      });
  };

  openCloseDialog = () => (this.isOpen = !this.isOpen);
}
