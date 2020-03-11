import { Component, OnInit, Input } from '@angular/core';
import { Cheese, CheeseService } from 'src/app/services/cheese.service';

@Component({
  selector: 'app-cheeseinfo',
  templateUrl: './cheeseinfo.component.html',
  styleUrls: ['./cheeseinfo.component.css']
})
export class CheeseinfoComponent implements OnInit {

  constructor(private cheeseService: CheeseService) { }

  @Input()CheeseId: number;
  CurrentCheese: Cheese;
  displayCheese: boolean = false;


  ngOnInit(): void {
    this.cheeseService.get(this.CheeseId).subscribe(data =>{
      this.CurrentCheese = data;
      this.displayCheese = true;
    });
  }
}
