import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './views/home/home.component';
import { CheeseComponent } from './views/cheese/cheese.component';


const routes: Routes = [
  { path: '', component: HomeComponent  },
  { path: 'cheese/:id', component: CheeseComponent},
  { path: '', redirectTo: '/home', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
